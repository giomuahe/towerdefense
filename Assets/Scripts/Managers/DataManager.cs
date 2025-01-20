using Assets.Scripts.DATA;
using MapConfigs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class DataManager : MonoBehaviour
    {
        private const string KEY_SAVE = "DATA_MAP_SAVE";

        private static DataManager inst;

        public static DataManager Instance {  get { if (inst == null) inst = new DataManager(); return inst; } }

        /// <summary>
        /// Lưu data về map, wave, info người chơi ở đầu mỗi wave
        /// </summary>
        /// <returns></returns>
        public bool BackUpdata()
        {
            SaveData dataSave = new SaveData();
            dataSave.Mapname = GameManager.Instance.MapManager.mapConfig.mapName;
            dataSave.CurrentHeart = GameManager.Instance.BattleManager.CurrentHeart();
            dataSave.CurrentWave = GameManager.Instance.BattleManager.CurrentWave();
            dataSave.CurrentGold = GameManager.Instance.BattleManager.CurrentGold();
            var dictTurret = GameManager.Instance.MapManager.GetTurretBases();
            Dictionary<int, TurretData> lsTurretSave = new Dictionary<int, TurretData>();
            var keyList = dictTurret.Keys;
            foreach ( var key in keyList)
            {
                TurretBase tur = dictTurret[key];
                TurretType turetType = TurretType.Base;
                if(tur != null && tur.gameObject!= null)
                {
                    turetType = tur.GetTurretType();
                    //Turret turretInfo = tur.gameObject.GetComponent<Turret>();
                    //turetType = turretInfo.TurretType;
                }
                TurretData data = new TurretData()
                {
                    X = tur.Position.x,
                    Y = tur.Position.y,
                    Z = tur.Position.z,
                    TurretType = turetType
                };
                lsTurretSave.Add(key, data);
            }
            dataSave.TurretInfo = lsTurretSave;
            string dataSaveTxt = JsonConvert.SerializeObject(dataSave);
            Debug.Log("SAVE_INFO : " + dataSaveTxt);
            PlayerPrefs.SetString(KEY_SAVE, dataSaveTxt);
            PlayerPrefs.Save();
            return true;
        }

        public SaveData GetDataSave()
        {
            string dataSaveTxt = PlayerPrefs.GetString(KEY_SAVE);
            if (!string.IsNullOrEmpty(dataSaveTxt)){
                SaveData data = JsonConvert.DeserializeObject<SaveData>(dataSaveTxt);
                if(data != null)
                    return data;
            }
            return null;
        }

        public void RemoveAllDataSave()
        {
            PlayerPrefs.DeleteKey(KEY_SAVE);
            PlayerPrefs.Save();
        }
    }
}
