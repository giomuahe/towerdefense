using System.Collections.Generic;
using UnityEngine;

namespace MapConfigs
{
    [CreateAssetMenu(fileName = "New Wave", menuName = "TowerDefense/WaveConfig")]
    [System.Serializable]
    public class WaveConfig : ScriptableObject
    {
        public List<EnemySpawnConfig> enemies;
        
        [System.Serializable]
        public class EnemySpawnConfig
        {
            public EnemyType type;
            public int amount;
            public float spawnDelay;
        }

    }
    
    public enum EnemyType
    {
        DroneScout,
        MechBrute,
        StealthInfiltrator,
        EMPDrone,
        CyberLeviathan
    }
}
