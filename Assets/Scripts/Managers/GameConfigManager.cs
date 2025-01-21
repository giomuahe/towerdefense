using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager
{
    public const int SECOND_BUILD_EXPIRED = 60; //số giây quy định được dùng để build turret

    private Dictionary<int, EnemyConfig> enemiesConfigDictionary;

    public void LoadEnemiesConfig()
    {
        enemiesConfigDictionary = new Dictionary<int, EnemyConfig>();
        EnemyConfig[] enemiesConfig = Resources.LoadAll<EnemyConfig>("Enemy");
        foreach(var enemyConfig in enemiesConfig)
        {
            if (enemiesConfigDictionary.ContainsKey((int)enemyConfig.EnemyType))
            {
                continue;
            }
            enemiesConfigDictionary.Add((int)enemyConfig.EnemyType, enemyConfig);
            Resources.UnloadAsset(enemyConfig);
        }
    }

    public EnemyConfig GetEnemyConfig(int enemyID)
    {
        return enemiesConfigDictionary[enemyID];
    }
}
