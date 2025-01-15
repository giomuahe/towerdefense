using MapConfigs;
using System.Collections.Generic;
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

        public void SpawnEnemies(WaveConfig waveConfig, Vector3 spawnPos)
        {
            StartCoroutine(spawner.WaveSpawn(waveConfig, spawnPos));
        }

        public void SendDamage(int enemyDataID, int damgage)
        {
            if (!dictionaryOfEnemiesOnMap.ContainsKey(enemyDataID))
            {
                Debug.Log("Wrong ID");
                return;
            }
            dictionaryOfEnemiesOnMap[enemyDataID].OnHit(damgage);
        }
    }
}
