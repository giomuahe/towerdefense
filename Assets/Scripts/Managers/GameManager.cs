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

    private int waveNumericalOrder;
    
    [SerializeField]
    private EnemySpawner enemySpawner;

    public static GameManager Instance {  get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
            Debug.LogError("NotFund UIManager");

        EnemyManager.Instance.Init();

        if (PoolManager != null)
        {
            PoolManager.CreateAllPool();
        }
        else
        {
            PoolManager = new PoolManager();
            PoolManager.CreateAllPool();
        }

        if(enemySpawner != null)
        {
            enemySpawner = new EnemySpawner();
        }
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
        StartCoroutine(enemySpawner.WaveSpawn(waveConfig,spawnPos));
    }

    public void NotifyOnHitTurret()
    {

    }
    #endregion
}
