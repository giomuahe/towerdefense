using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MapConfig
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "TowerDefense/MapConfig", order = 1)]
    public class MapConfig : ScriptableObject
    {
        [System.Serializable]
        public class SpawnPosition
        {
            public Vector3 position;
            public Vector3 rotation;
        }
            public string mapName;
            public List<Vector3> turretPositions;
            public SpawnPosition mainGatePosition;
            public SpawnPosition spawnGatePosition;
            public List<WaveConfig> waves;
     }
}