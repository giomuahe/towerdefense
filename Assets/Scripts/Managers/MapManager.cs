using UnityEngine;
using System.Collections.Generic;
using MapConfigs;

namespace Managers
{
    public class MapManager : MonoBehaviour
    {
        public MapConfig mapConfig;
        public Dictionary<Vector3, GameObject> PlacedTurrets = new Dictionary<Vector3, GameObject>();

        public MapConfig.SpawnPosition GetMainGatePosition()
        {
            return mapConfig.mainGatePosition;
        }

        public MapConfig.SpawnPosition GetSpawnGatePosition()
        {
            return mapConfig.spawnGatePosition;
        }

        public WaveConfig GetWaveConfig(int waveIndex)
        {
            if (waveIndex >= 0 && waveIndex < mapConfig.waves.Count)
            {
                return mapConfig.waves[waveIndex];
            }
            return null;
        }

        public int GetTotalWave()
        {
            return mapConfig.waves.Count;
        }

        public void AddTurret(Vector3 position, GameObject turret)
        {
            if (!PlacedTurrets.ContainsKey(position))
            {
                PlacedTurrets.Add(position, turret);
                Debug.Log($"Adding new turret at {position}");
            }
            else
            {
                Debug.LogWarning($"Already placed turret at {position}");
            }
        }

        public void RemoveTurret(Vector3 position)
        {
            if (PlacedTurrets.ContainsKey(position))
            {
                PlacedTurrets.Remove(position);
                Debug.Log($"Removing turret at {position}");
            }
            else
            {
                Debug.LogWarning($"No placed turret at {position}");
            }
        }

        public bool HasTurret(Vector3 position)
        {
            return PlacedTurrets.ContainsKey(position);
        }

        public GameObject GetTurret(Vector3 position)
        {
            if (PlacedTurrets.TryGetValue(position, out GameObject turret))
            {
                return turret;
            }
            return null;
        }

        public Dictionary<int, Vector3> GetTurretBasePositions()
        {
            return mapConfig.TurretBasePositions;
        }

        public bool IsTurretBase(int turretBaseId)
        {
            return mapConfig.TurretBasePositions.ContainsKey(turretBaseId);
        }

        public Dictionary<Vector3, GameObject> GetAllTurrets()
        {
            return new Dictionary<Vector3, GameObject>(PlacedTurrets);
        }

        public List<Vector3> GetWaypoints()
        {
            return mapConfig.waypoints;
        }
    }
}
