using Managers;
using MapConfigs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //UI
    public UIManager UIManager;

    public MapManager MapManager;

    public PoolManager PoolManager;

    public TurretManager TurretManager;

    private int waveNumericalOrder;
    
    [SerializeField]
    private EnemyManager enemyManager;

    public static GameManager Instance {  get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if(PoolManager)
            PoolManager.CreateAllPool();
    }

    private void Start()
    {
        if(MapManager == null)
        {
            MapManager = new MapManager();
        }

        if (UIManager)
            UIManager.Init();
        else
            Debug.LogError("NotFound UIManager");

        //if (PoolManager != null)
        //{
        //    PoolManager.CreateAllPool();
        //}
        //else
        //{
        //    PoolManager = new PoolManager();
        //    PoolManager.CreateAllPool();
        //}
    }

    #region UI
    /// UI -------------
    #region Lobby

    #endregion Lobby

    #region GamePlay

    #endregion GamePlay
    /// UI -------------
    #endregion UI

    #region Enemy
    private void CreateWave()
    {
        WaveConfig waveConfig = MapManager.GetWaveConfig(waveNumericalOrder);
        Vector3 spawnPos = MapManager.GetSpawnGatePosition().position;
        enemyManager.SpawnEnemies(waveConfig, spawnPos);
    }

    public void NotifyOnHitTurret()
    {

    }
    #endregion
}
