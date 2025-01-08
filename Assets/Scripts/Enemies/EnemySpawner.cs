using Managers;
using MapConfigs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static int EnemyInGameID = 1000;

    private void SpawnEnemy(EnemyConfig enemyConfig, Vector3 spawnPos)
    {
        GameObject spawnObj = Instantiate(enemyConfig.EnemyPrefab, spawnPos, Quaternion.identity);
        List<Vector3> moveLocations = EnemyManager.Instance.moveLocations;
        spawnObj.GetComponent<EnemyBase>().SetUp(enemyConfig, moveLocations, EnemyInGameID);
        EnemyInGameID++;
    }

    public IEnumerator WaveSpawn(WaveConfig waveConfig, Vector3 spawnPos)
    {
        foreach(var enemy in waveConfig.enemies)
        {
            float delayTime = enemy.spawnDelay;
            EnemyConfig enemyConfig = GameConfigManager.Instance.GetEnemyConfig((int)enemy.type);
            for(int i = 0; i < enemy.amount; i++)
            {
                yield return new WaitForSeconds(delayTime);
                SpawnEnemy(enemyConfig, spawnPos);
            }
        }
    }
}

