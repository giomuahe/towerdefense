using UnityEngine;
using System.Collections.Generic;
using MapConfigs;
using Assets.Scripts.DATA;

namespace Managers
{
    public class MapManager: MonoBehaviour
    {
        public MapConfig mapConfig;
        private readonly Dictionary<int, TurretBase> _turretBases = new Dictionary<int, TurretBase>();

        public static MapManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RegisterTurretBase(int baseId, TurretBase turretBase)
        {
            if (!_turretBases.ContainsKey(baseId))
            {
                _turretBases.Add(baseId, turretBase);
                Debug.Log($"Registered TurretBase {baseId} at position {turretBase.Position}");
            }
            else
            {
                Debug.LogWarning($"Already registered TurretBase {baseId}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="turret"></param>
        public void UpdateTurret(int baseId, GameObject turret)
        {
            if (!_turretBases.TryGetValue(baseId, out var turretBase))
            {
                Debug.LogError("Turret base is missing or invalid!");
                return;
            }

            turretBase.Turret = turret;
        }

        public bool HasTurret(int baseId)
        {
            TurretBase turretBase = _turretBases[baseId];
            return turretBase != null && turretBase.Turret != null;
        }

        public TurretBase GetTurretBase(int baseId)
        {
            if (_turretBases.TryGetValue(baseId, out TurretBase turretBase))
            {
                return turretBase;
            }
            return null;
        }

        public Dictionary<int, TurretBase> GetTurretBases()
        {
            return _turretBases;
        }

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
        
        public List<Vector3> GetWaypoints()
        {
            return mapConfig.waypoints;
        }

        public int GetMainGateHealth()
        {
            return mapConfig.mainGateHealth;
        }

        public int GetStartingGold()
        {
            return mapConfig.startingGold;
        }
        
        /// <summary>
        /// Enemy bi tieu diet
        /// </summary>
        public void OnEnemyDie(){

        }

        /// <summary>
        /// Enemy den diem cuoi
        /// </summary>
        public void OnEnemyPass(){

        }

        
    }
}
