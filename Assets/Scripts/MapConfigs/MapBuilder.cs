using Managers;
using UnityEngine;

namespace MapConfigs
{
    public class MapBuilder : MonoBehaviour
    {
        public GameObject turretSpotPrefab;
        public GameObject mainGatePrefab;
        public GameObject spawnGatePrefab;
        public MapConfig mapConfig;
        public MapManager mapManager;

        private void OnEnable()
        {
            CreateMainGates();
            CreateSpawnGates();
            CreateTurretBases();
        }

        private void CreateTurretBases()
        {
            if (turretSpotPrefab == null)
            {
                Debug.LogError($"No turret spot prefab assigned");
                return;
            }
            
            for (int i = 0; i < mapConfig.turretBasePositions.Count; i++)
            {
                Vector3 position = mapConfig.turretBasePositions[i];
                
                GameObject turretSpot = Instantiate(turretSpotPrefab, position, Quaternion.identity);
                turretSpot.name = $"TurretSpot_{i}";
                
                TurretBase turretBase = turretSpot.GetComponent<TurretBase>();
                if (turretBase != null)
                {
                    turretBase.Initialize(i, position);
                    mapManager.RegisterTurretBase(i, turretBase);
                }
            }
        }

        private void CreateMainGates()
        {
            Quaternion gateRotation = Quaternion.Euler(mapConfig.spawnGatePosition.rotation);
            Instantiate(mainGatePrefab, mapConfig.mainGatePosition.position, gateRotation);
        }

        private void CreateSpawnGates()
        {
            Quaternion spawnRotation = Quaternion.Euler(mapConfig.spawnGatePosition.rotation);
            Instantiate(spawnGatePrefab, mapConfig.spawnGatePosition.position, spawnRotation);
        }
    }
}
