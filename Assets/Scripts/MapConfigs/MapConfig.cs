using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        
        [Header("Map Name")]
        public string mapName;
        
        [Header("Gate Settings")]
        public SpawnPosition mainGatePosition;
        public SpawnPosition spawnGatePosition;
        
        [Header("Turret Base Positions")]
        public List<Vector3> turretBasePositions =  new List<Vector3>();
        
        [Header("Wave Settings")]
        public List<WaveConfig> waves;
        
        [Header("Waypoints Settings")]
        public List <Vector3> waypoints;
        
     }
}