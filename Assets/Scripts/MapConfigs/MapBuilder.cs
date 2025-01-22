using Managers;
using System.Collections.Generic;
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

        public List<GameObject> spawnGates = new List<GameObject>();

        private void OnEnable()
        {
            CreateMainGates();
            //CreateSpawnGates();
            CreateTurretBases();
            CreateMultipleSpawnGates();
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
            GameObject mainGateObj = Instantiate(mainGatePrefab, mapConfig.mainGatePosition.position, gateRotation);
            mainGateObj.transform.localScale = mapConfig.mainGatePosition.scale;
        }

        private void CreateSpawnGates()
        {
            Quaternion spawnRotation = Quaternion.Euler(mapConfig.spawnGatePosition.rotation);
            Instantiate(spawnGatePrefab, mapConfig.spawnGatePosition.position, spawnRotation);
        }

        private void CreateMultipleSpawnGates()
        {
            if(mapConfig == null || spawnGatePrefab == null)
            {
                Debug.Log("Map Config or Prefab were not assigned");
                return;
            }

            if(mapConfig.multipleSpawnGatePositions != null)
            {
                foreach (var gateConfig in mapConfig.multipleSpawnGatePositions)
                {
                    GameObject gateObj = Instantiate(spawnGatePrefab, gateConfig.postion, Quaternion.Euler(gateConfig.rotation));

                    gateObj.transform.localScale = gateConfig.scale;

                    spawnGates.Add(gateObj);

                    Debug.Log($"Spawned Gate at position {gateConfig.postion}, rotation {gateConfig.rotation}, scale {gateConfig.scale}");
                }
            }
        }
    }
}
