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
    [Serializable]
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
            //Debug.Log(string.Format("GOLD_CHANGE Gold = {0}, des = {1}", goldChange, description));
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
            //Lưu data backup
            DataManager.Instance.BackUpdata();
            Debug.Log("DATA_BACKUP");

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
            //Chỉ khi combat mới pause được
            if (GAME_STATE != EGAMESTATE.COMBAT)
                return false;

            IsPause = !IsPause;
            if (IsPause)
                GAME_STATE = EGAMESTATE.PAUSE;
            else
                GAME_STATE = EGAMESTATE.COMBAT;
            Time.timeScale = IsPause ? 0f : 1f;

            return IsPause;
        }

        #endregion

        #region System Auto Control

        /// <summary>
        /// Bắt đầu wave mới, vào state building
        /// </summary>
        private void BeginNewWave()
        {
            bool isOpenNewWave = GameManager.Instance.WaveManager.AdvanceToNextWave(out currentWave);
            if ( !isOpenNewWave ) {
                string error = string.Format("CANT_CREATE_NEW_WAVE, Map : {0}, curWave : {1}", mapname, currentWave);
                GameManager.Instance.UIManager.ShowPopup(EPOPUP.CONFIRM_POPUP, EMESSAGETYPE.ERROR, "Lỗi", error);
                return;
            }

            if (currentWave == 0)
            {
                //Lấy max wave trong map
                maxWaveInMap = GameManager.Instance.WaveManager.GetWaves().Count;
                GameManager.Instance.WriteDebug("MAX_WAVE " + maxWaveInMap);
            }

            //Cập nhật State sang Building
            GAME_STATE = EGAMESTATE.BUILDING;
            //Lấy số lượng quái sẽ có trong turn
            numEnemyInCurWave = GameManager.Instance.WaveManager.GetTotalEnemyInCurretWave();
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
            BeginNewWave();
        }

        /// <summary>
        /// Thắng
        /// </summary>
        private void EndWinGame()
        {
            GAME_STATE = EGAMESTATE.END_GAME;
            EndGame?.Invoke(true);
        }

        /// <summary>
        /// Thua
        /// </summary>
        private void EndLostGame()
        {
            GAME_STATE = EGAMESTATE.END_GAME;
            EndGame?.Invoke(false);
        }

        /// <summary>
        /// Gọi đến mỗi khi chết 1 enemy
        /// </summary>
        public void OnEnemyDie(long goldBonus)
        {
            Debug.Log("ENEMY_DIE " + goldBonus);
            if (GAME_STATE == EGAMESTATE.COMBAT) {
                UpdateGold(goldBonus, "Kill Enemy");
                numEnemyInCurWave -= 1;
                if (curretHeart <= 0)
                    EndLostGame();
                if (numEnemyInCurWave <= 0)
                {
                    EndWave();
                }
            }
            else
            {
                Debug.Log("CANT_UPDATE_ENEMY_DIE_BECAUSE_END_GAME");
            }
        }

        /// <summary>
        /// Gọi đến mỗi khi 1 enemy thoát
        /// </summary>
        public void OnEnemyEscape()
        {
            Debug.Log("ENEMY_ESCAPE");
            if (GAME_STATE == EGAMESTATE.COMBAT)
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
            else
            {
                Debug.Log("CANT_UPDATE_ENEMY_ESCAPE_BECAUSE_NOT_IN_COMBAT");
            }

        }

        /// <summary>
        /// Kiểm tra xem có thể xây trụ không
        /// </summary>
        /// <param name="turretCost"> giá của trụ, yc lớn hơn 0</param>
        /// <param name="errorMessage">lỗi</param>
        /// <returns></returns>
        public bool IsCanBuildTurret(long turretCost, string turretName, out string errorMessage)
        {
            errorMessage = "unknow_error";
            bool result = false;
            if(turretCost > currentGold)
            {
                errorMessage = "Số dư không đủ !";
                return false;
            }
            long goldDeduct = -1* Math.Abs(turretCost);
            result = UpdateGold(goldDeduct, "Build Turret " + turretName);
            if (!result) { errorMessage = "Trừ tiền thất bại, vui lòng thử lại sau !"; }
            return result;
        }

        #endregion

        public int CurrentHeart()
        {
            return curretHeart;
        }

        public long CurrentGold()
        {
            return currentGold;
        }

        public int CurrentWave()
        {
            return currentWave;
        }
    }
}
