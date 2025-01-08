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
        
        [System.Serializable]
        public struct TurretBaseEntry
        {
            public int BaseId;
            public Vector3 Position;
        }
        public Dictionary<int, Vector3> TurretBasePositions = new Dictionary<int, Vector3>();
        public string mapName;
        public List<TurretBaseEntry> TurretBaseEntries =  new List<TurretBaseEntry>();
        public SpawnPosition mainGatePosition;
        public SpawnPosition spawnGatePosition;
        public List<WaveConfig> waves;
        public List <Vector3> waypoints;

        public void InitializeDictionary()
        {
            TurretBasePositions.Clear();
            foreach(var entry in TurretBaseEntries)
                if (!TurretBasePositions.ContainsKey(entry.BaseId))
                {
                    TurretBasePositions.Add(entry.BaseId, entry.Position);
                }
        }
     }
}