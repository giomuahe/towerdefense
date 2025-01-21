using MapConfigs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class EnemyManager: MonoBehaviour
    {
        [SerializeField]
        private EnemySpawner spawner;
        private Dictionary<int, EnemyBase> dictionaryOfEnemiesOnMap = new();
        
        public void AddEnemiesToDic(int id, EnemyBase enemy)
        {
            dictionaryOfEnemiesOnMap.Add(id, enemy);
        }

        public void RemoveEnemyFromDic(int id)
        {
            dictionaryOfEnemiesOnMap.Remove(id);
        }

        public void SpawnEnemies(WaveConfig waveConfig, Vector3 spawnPos)
        {
            StartCoroutine(spawner.WaveSpawn(waveConfig, spawnPos));
        }

        public void SendDamage(int enemyDataID, int damgage, out bool isDie)
        {
            isDie = false;
            if (!dictionaryOfEnemiesOnMap.ContainsKey(enemyDataID))
            {
                Debug.Log("Wrong ID TowerSend : " + enemyDataID + ", Current : " + JsonConvert.SerializeObject(dictionaryOfEnemiesOnMap.Keys.ToList()));
                isDie = true;
                return;
            }else{
                Debug.Log("OK ID TowerSend : " + enemyDataID + ", Current : " + JsonConvert.SerializeObject(dictionaryOfEnemiesOnMap.Keys.ToList()));
            }
            dictionaryOfEnemiesOnMap[enemyDataID].OnHit(damgage, out isDie);
        }

        public void RestartEnemyManager()
        {
            if (dictionaryOfEnemiesOnMap.Count == 0) return;
            foreach(EnemyBase enemy in dictionaryOfEnemiesOnMap.Values)
            {
                if(enemy != null && enemy.gameObject != null)
                    Destroy(enemy.gameObject);
            }
            dictionaryOfEnemiesOnMap.Clear();
        }
    }
}
