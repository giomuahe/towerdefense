using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace MapConfigs
{
    public class MapBuilder : MonoBehaviour
    {
        public GameObject turretSpotPrefab;
        public GameObject mainGatePrefab;
        public GameObject spawnGatePrefab;
        public MapConfig mapConfig;

        private void Start()
        {
            mapConfig.InitializeDictionary();

            if (mapConfig.TurretBasePositions == null || mapConfig.TurretBasePositions.Count == 0)
            {
                Debug.LogError($"No turret base positions defined!");
                return;
            }
            
            foreach (var turret in mapConfig.TurretBasePositions)
            {
                int baseId = turret.Key;
                Vector3 position = turret.Value;
                
                Debug.Log("Turret position: " + turret);
                Instantiate(turretSpotPrefab, position, Quaternion.identity);
            }
            
            Quaternion gateRotation = Quaternion.Euler(mapConfig.spawnGatePosition.rotation);
            Instantiate(mainGatePrefab, mapConfig.mainGatePosition.position, gateRotation);
            
            Quaternion spawnRotation = Quaternion.Euler(mapConfig.spawnGatePosition.rotation);
            Instantiate(spawnGatePrefab, mapConfig.spawnGatePosition.position, spawnRotation);
            
            Debug.Log("Map built successfully!");
        }
    }
}
