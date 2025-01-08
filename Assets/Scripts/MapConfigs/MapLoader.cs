using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MapConfigs
{
    [CreateAssetMenu(fileName = "NewMapConfig", menuName = "TowerDefense/MapConfig", order = 1)]
    public class MapConfig : ScriptableObject
    {
        [System.Serializable]
        public class SpawnPosition
        {
            public Vector3 position;
            public Vector3 rotation;
        }
            public string mapName;
            public SpawnPosition mainGatePosition;
            public SpawnPosition spawnGatePosition;
            public Dictionary<int, Vector3> TurretBasePositions;
            public List<WaveConfig> waves;
            public List <Vector3> waypoints;
     }
}