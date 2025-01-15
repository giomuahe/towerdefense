using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager
{
    private Dictionary<int, EnemyConfig> enemiesConfigDictionary;

    public void LoadEnemiesConfig()
    {
        enemiesConfigDictionary = new Dictionary<int, EnemyConfig>();
        EnemyConfig[] enemiesConfig = Resources.LoadAll<EnemyConfig>("Enemy");
        foreach(var enemyConfig in enemiesConfig)
        {
            enemiesConfigDictionary.Add((int)enemyConfig.EnemyType, enemyConfig);
            Resources.UnloadAsset(enemyConfig);
        }
    }

    public EnemyConfig GetEnemyConfig(int enemyID)
    {
        return enemiesConfigDictionary[enemyID];
    }
}
