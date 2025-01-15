using Managers;
using MapConfigs;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //UI
    public UIManager UIManager;

    public MapManager MapManager;

    public WaveManager WaveManager;

    public PoolManager PoolManager;

    public GameConfigManager GameConfigManager;

    public int WaveNumericalOrder = 0;
    
    public TurretManager TurretManager;

    [SerializeField]
    public EnemyManager EnemyManager;

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
        if(GameConfigManager is null)
        {
            GameConfigManager = new GameConfigManager();
        }
        GameConfigManager.LoadEnemiesConfig();
    }

    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {

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
        //CreateWave();
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
    public void CreateWave()
    {
        WaveConfig waveConfig = MapManager.GetWaveConfig(WaveNumericalOrder);
        Vector3 spawnPos = MapManager.GetSpawnGatePosition().position;
        print("Createwave " + JsonConvert.SerializeObject(waveConfig) + ",spaw " + spawnPos);
        EnemyManager.SpawnEnemies(waveConfig, spawnPos); 
    }

    public void NotifyOnHitTurret()
    {

    }
    #endregion

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        print("LOADED SCENE " + scene.name);
        if(scene.name.Equals("MainMenu"))
            return;
        GameObject mapInfo =  GameObject.Find("Map Manager");
        if(mapInfo)
            MapManager = mapInfo.GetComponent<MapManager>();
        else
            print("Not found Map Manager");
    }
}
