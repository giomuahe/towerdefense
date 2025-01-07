using System.Collections.Generic;
using UnityEngine;

namespace MapConfig
{
    [CreateAssetMenu(fileName = "New Wave", menuName = "TowerDefense/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        [System.Serializable]
        public class EnemySpawnConfig
        {
            public string type;
            public int amount;
            public float spawnDelay;
        }

        public List<EnemySpawnConfig> enemies;
    }
}
