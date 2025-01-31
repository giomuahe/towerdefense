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
        //Dictionary contains 2 group of waypoint lists: Key = 0 : groupA, Key = 1 : groupB
        private readonly Dictionary<int, List<Vector3>> _waypointsDict = new Dictionary<int, List<Vector3>>();
        private readonly List<GameObject> spawnGates = new List<GameObject>();

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
            CreateWaypointsDictionary();
        }

        public void RegisterTurretBase(int baseId, TurretBase turretBase)
        {
            if (!_turretBases.ContainsKey(baseId))
            {
                _turretBases.Add(baseId, turretBase);
            }
        }

        private void CreateWaypointsDictionary()
        {
            _waypointsDict.Clear();
            if (mapConfig == null)
            {
                return;
            }

            foreach (WaypointsGroup group in mapConfig.waypointsGroups)
            {
                if (group != null)
                {
                    if (group.waypoints != null && group.waypoints.Count > 0)
                    {
                        _waypointsDict[group.groupId] = new List<Vector3>(group.waypoints);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseId"></param>
        /// <param name="turret"></param>
        public void UpdateTurret(int baseId, GameObject turret, TurretType turretType)
        {
            if (!_turretBases.TryGetValue(baseId, out var turretBase))
            {
                Debug.LogError("Turret base is missing or invalid!");
                return;
            }
            turretBase.TurretType = turretType;
            turretBase.Turret = turret;
            _turretBases[baseId] = turretBase;
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

        public GameObject FindSpawnGateById(int spawnGateId)
        {
            foreach (var gate in spawnGates)
            {
                var spawnGateConfig = gate.GetComponent<SpawnGateConfig>();
                if(spawnGateConfig != null && spawnGateConfig.gateId == spawnGateId)
                {
                    return gate;
                }
            }
            return null;
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
        
        //public List<Vector3> GetWaypoints()
        //{
        //    return mapConfig.waypointsGroupA;
        //}

        //public List<Vector3> GetWaypointsB()
        //{
        //    return mapConfig.waypointsGroupB;
        //}

        //public List<Vector3> GetRandomWaypoints()
        //{
        //    int groupKey = Random.Range(0, 2);
        //    Debug.Log("RANDOM " + groupKey);
        //    return _waypointsDict[groupKey];
        //}

        public List<Vector3> GetRandomWaypointsGroup()
        {
            if (_waypointsDict.Count == 0)
            {
                Debug.LogError("No waypoints group available!");
                return null;
            }
            
            List<int> groupKeys = new List<int>(_waypointsDict.Keys);
            
            int randomIndex = Random.Range(0, groupKeys.Count);
            int selectedGroupKey = groupKeys[randomIndex];

            if (_waypointsDict.TryGetValue(selectedGroupKey, out List<Vector3> waypointList))
            {
                return waypointList;
            }
            
            return new List<Vector3>();
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


        public int GetStartingGold()
        {
            //TODO
            return mapConfig.startingGold;
        }

        public int GetMainGateHealth()
        {
            //TODO
            return mapConfig.nexusHealth;
        }
    }
}
