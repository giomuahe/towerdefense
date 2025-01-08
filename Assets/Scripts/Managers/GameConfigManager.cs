using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager
{
    private Dictionary<int, EnemyConfig> enemiesConfigDictionary;
    public static GameConfigManager Instance;
   
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = new GameConfigManager();
        }
        LoadEnemiesConfig();
    }

    private void LoadEnemiesConfig()
    {
        EnemyConfig[] enemiesConfig = Resources.LoadAll<EnemyConfig>("Enemy");
        foreach(var enemyConfig in enemiesConfig)
        {
            enemiesConfigDictionary.Add(enemyConfig.EnemyID, enemyConfig);
            Resources.UnloadAsset(enemyConfig);
        }
    }

    public EnemyConfig GetEnemyConfig(int enemyID)
    {
        return enemiesConfigDictionary[enemyID];
    }
}
