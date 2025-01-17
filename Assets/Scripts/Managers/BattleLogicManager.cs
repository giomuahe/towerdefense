using Assets.Scripts.Enums;
using Managers;
using MapConfigs;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Quản lý luồng logic khi vào scene "MAP"
    /// </summary>
    public class BattleLogicManager
    {
        //Core variable
        private string mapname = "";
        private int currentWave = 0;
        private long currentGold = 0;
        private int curretHeart = 0;

        //Temp variable
        public EGAMESTATE GAME_STATE = EGAMESTATE.NONE;
        private int numEnemyInCurWave = 0;
        private int maxWaveInMap = 0;
        private bool IsPause = false;
        private int secRemainTime;

        //Event 
        public event Action<int, int> OnWaveChanged;
        public event Action<long> OnGoldChanged;
        public event Action<int> OnHeartChanged;
        public event Action<long> UpdateGameTime;
        public event Action<bool> EndGame;
        public event Action<int> OnTimeChanged;

        public BattleLogicManager(string Mapname, long initGold, int initHeart)
        {
            mapname = Mapname;
            UpdateWave(0);
            UpdateGold(initGold);
            UpdateHeart(initHeart);
            //Bắt đầu luôn là building
            BeginNewWave();
        }

        public void RefeshActionUI()
        {
            UpdateWave(0);
            UpdateGold(0);
            UpdateHeart(0);
        }

        /// <summary>
        /// Khi thay đổi wave gọi vào đây để push sang UI
        /// </summary>
        /// <param name="wave"></param>
        public void UpdateWave(int wave)
        {
            currentWave += wave;
            OnWaveChanged?.Invoke(currentWave, maxWaveInMap);
        }

        /// <summary>
        /// Cập nhật vàng của người chơi
        /// </summary>
        /// <param name="goldChange">Có thể âm hoặc dương</param>
        public bool UpdateGold(long goldChange, string description = "")
        {
            Debug.Log(string.Format("GOLD_CHANGE Gold = {0}, des = {1}", goldChange, description));
            long oldGold = currentGold;
            try
            {
                long newGold = currentGold + goldChange;
                if(newGold < 0)
                {
                    throw new Exception("Không đủ vàng");
                }
                currentGold = newGold;
                OnGoldChanged?.Invoke(currentGold);
                return true;
            }
            catch (Exception ex) {
                currentGold = oldGold;
                Debug.Log(string.Format("Không thể thực hiện giao dịch : {0}", ex.Message));
                return false;
            }
        }
    
        /// <summary>
        /// Cập nhật số mạng còn lại của người chơi
        /// </summary>
        /// <param name="heart"></param>
        public void UpdateHeart(int heart)
        {
            curretHeart += heart;
            OnHeartChanged?.Invoke(curretHeart);
        }

        #region Player Control

        /// <summary>
        /// Sinh quái, Vào state combat
        /// </summary>
        public void CreateWave()
        {
            //Cập nhật State sang Combat
            GAME_STATE = EGAMESTATE.COMBAT;
            //Lấy config wave
            WaveConfig waveConfig = GameManager.Instance.MapManager.GetWaveConfig(currentWave);
            Vector3 spawnPos = GameManager.Instance.MapManager.GetSpawnGatePosition().position;
            GameManager.Instance.WriteDebug("Createwave " + JsonConvert.SerializeObject(waveConfig) + ",spaw " + spawnPos);
            //Sinh quái
            GameManager.Instance.EnemyManager.SpawnEnemies(waveConfig, spawnPos);
        }

        public bool ChangePause()
        {
            IsPause = !IsPause;
            return IsPause;
        }

        #endregion

        #region System Auto Control

        /// <summary>
        /// Bắt đầu wave mới, vào state building
        /// </summary>
        private void BeginNewWave()
        {
            if(currentWave == 0)
            {
                //Lấy max wave trong map
                maxWaveInMap = GameManager.Instance.WaveManager.GetWaves().Count;
                GameManager.Instance.WriteDebug("NUM_WAVE " + maxWaveInMap);
            }

            //Cập nhật State sang Building
            GAME_STATE = EGAMESTATE.BUILDING;
            //Lấy số lượng quái sẽ có trong turn
            numEnemyInCurWave = GameManager.Instance.WaveManager.GetTotalEnemyCount();
            //Bắt đầu đếm thời gian xây dựng
            secRemainTime = GameConfigManager.SECOND_BUILD_EXPIRED;
            GameManager.Instance.StartCoroutine(CountdownCoroutine());
        }

        private IEnumerator CountdownCoroutine()
        {
            while(secRemainTime > 0 && GAME_STATE == EGAMESTATE.BUILDING)
            {
                yield return new WaitForSeconds(1);
                secRemainTime -- ;
                OnTimeChanged?.Invoke(secRemainTime);
            }
            //Đếm xong
            OnTimeChanged?.Invoke(0);
            //Tự động chạy wave mới
            if(GAME_STATE == EGAMESTATE.BUILDING)
                CreateWave();
        }

        /// <summary>
        /// Kết thúc wave
        /// </summary>
        private void EndWave()
        {
            if (GameManager.Instance.WaveManager.AreAllWavesCompleted()) {
                EndWinGame();
                return;
            }
            //Mở wave mới
            if (GameManager.Instance.WaveManager.AdvanceToNextWave())
            {
                BeginNewWave();
            }
            else
            {
                Debug.LogError(string.Format("CANT_CREATE_NEW_WAVE, Map : {0}, curWave : {1}", mapname, currentWave));
            }
        }

        /// <summary>
        /// Thắng
        /// </summary>
        private void EndWinGame()
        {
            EndGame?.Invoke(true);
        }

        /// <summary>
        /// Thua
        /// </summary>
        private void EndLostGame()
        {
            EndGame?.Invoke(false);
        }

        /// <summary>
        /// Gọi đến mỗi khi chết 1 enemy
        /// </summary>
        public void OnEnemyDie()
        {
            numEnemyInCurWave -= 1;
            if(curretHeart<=0)
                EndLostGame();
            if(numEnemyInCurWave <= 0)
            {
                EndWave();
            }
        }

        /// <summary>
        /// Gọi đến mỗi khi 1 enemy thoát
        /// </summary>
        public void OnEnemyEscape()
        {
            numEnemyInCurWave -= 1;
            UpdateHeart(-1);
            if (curretHeart <= 0)
                EndLostGame();
            if (numEnemyInCurWave <= 0)
            {
                EndWave();
            }
        }

        #endregion
    }
}
