using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace MapConfig
{
    public class MapBuilder : MonoBehaviour
    {
        public GameObject turretSpotPrefab;
        public GameObject mainGatePrefab;
        public GameObject spawnGatePrefab;
        public MapConfig mapConfig;

        private void Start()
        {

            foreach (var turret in mapConfig.turretPositions)
            {
                Debug.Log("Turret position: " + turret);
                Instantiate(turretSpotPrefab, turret, Quaternion.identity);
            }
            
            Quaternion gateRotation = Quaternion.Euler(mapConfig.spawnGatePosition.rotation);
            Instantiate(mainGatePrefab, mapConfig.mainGatePosition.position, gateRotation);
            
            Quaternion spawnRotation = Quaternion.Euler(mapConfig.spawnGatePosition.rotation);
            Instantiate(spawnGatePrefab, mapConfig.spawnGatePosition.position, spawnRotation);
            
            Debug.Log("Map built successfully!");
        }
    }
}
