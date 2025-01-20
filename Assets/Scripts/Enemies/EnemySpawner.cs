using Managers;
using MapConfigs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;

    private int enemyInGameID;

    private void SpawnEnemy(EnemyConfig enemyConfig, Vector3 spawnPos)
    {
        GameObject spawnObj = Instantiate(enemyConfig.EnemyPrefab, spawnPos, Quaternion.identity);
        spawnObj.transform.SetParent(transform);
        List<Vector3> moveLocations = GameManager.Instance.MapManager.GetRandomWaypointsGroup();
        spawnObj.GetComponent<EnemyBase>().SetUp(enemyConfig, moveLocations, enemyInGameID);
        enemyManager.AddEnemiesToDic(enemyInGameID, spawnObj.GetComponent<EnemyBase>());
        enemyInGameID++;
    }

    public IEnumerator WaveSpawn(WaveConfig waveConfig, Vector3 spawnPos)
    {
        enemyInGameID = 1000;
        foreach(var enemy in waveConfig.enemies)
        {
            float delayTime = enemy.spawnDelay;
            EnemyConfig enemyConfig = GameManager.Instance.GameConfigManager.GetEnemyConfig((int)enemy.type);
            for(int i = 0; i < enemy.amount; i++)
            {
                yield return new WaitForSeconds(delayTime);
                SpawnEnemy(enemyConfig, spawnPos);
            }
        }
    }
}

