using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using Managers;
using MapConfigs;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //All Screen
    public UIManager UIManager;

    public PoolManager PoolManager;

    public GameConfigManager GameConfigManager;

    public TurretManager TurretManager;

    public EnemyManager EnemyManager;

    //Battle

    public MapManager MapManager;

    public WaveManager WaveManager;

    public BattleLogicManager BattleManager;

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
    }

    #region UI
    /// UI -------------
    #region Lobby

    #endregion Lobby

    #region GamePlay

    #endregion GamePlay
    /// UI -------------
    #endregion UI

    #region Battle

    private void BeginNewBattle(string Mapname)
    {
        //Load thông tin từ map mới
        MapManager = MapManager.Instance;
        if(MapManager == null)
            print("Not found MapManager");
        WaveManager = WaveManager.Instance;
        if (WaveManager == null)
            print("Not found WaveManager");
        else
            WaveManager.Init();

        long goldInit = 1000;
        int heartInit = 5;
        BattleManager = new BattleLogicManager(Mapname, goldInit, heartInit);
        UIManager.ShowScreen(ESCREEN.IN_BATTLE);
        Debug.Log("BEGIN_BATTLE");
    }

    public void CreateWave()
    {
        if (BattleManager == null)
            return;
        BattleManager.CreateWave();
    }

    public void OnEnemyDie(long goldBonus)
    {
        BattleManager?.OnEnemyDie();
    }

    public void OnEnemyEscape()
    {
        BattleManager?.OnEnemyEscape();
    }

    /// <summary>
    /// Người chơi chủ động thoát trận đánh -> không lưu
    /// </summary>
    public void PlayerEndBatte()
    {
        //Remove All save data

        //Reset info battle in game
        MapManager = null;
        WaveManager = null;
        BattleManager = null;
    }

    #endregion

    #region Enemy

    public void NotifyOnHitTurret()
    {

    }

    public void NotifyOnTakeDamaged()
    {

    }

    /// <summary>
    /// Thông báo khi sinh boss
    /// </summary>
    public void NotifyOnBossAppear()
    {

    }
    #endregion

    /// <summary>
    /// Event Load Scene xong
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("LOADED SCENE " + scene.name);
        if (scene.name.Equals("MainMenu"))
            return;
        string mapname = scene.name;
        BeginNewBattle(mapname);
    }

    public void WriteDebug(string msg)
    {
        Debug.Log(msg);
    }
}
