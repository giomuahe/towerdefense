using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static int EnemyInGameID = 1000;

    private void SpawnEnemy(int enemyID, GameObject enemyPrefab, Vector3 spawnPos, List<Vector3> moveLocations)
    {
        // tìm enemy config theo ID
        GameObject spawnObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        //SetUp Enemy theo id tim duoc
        //spawnObj.GetComponent<EnemyBase>().SetUp();
    }

    // WaveSpawn()
}

[Serializable]
public class EnemySpawnConfig
{
    //Tạo id để tìm prefab
    //public GameObject enemyPrefab;
    public int numberInstance;
}
