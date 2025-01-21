using Assets.Scripts.DATA;
using Assets.Scripts.Enums;
using Assets.Scripts.Managers;
using Managers;
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

    public bool isContinueOldMap = false;

    public SaveData dataBackUp;

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

        //Kiểm tra xem có file lưu không
        dataBackUp = DataManager.Instance.GetDataSave();
        if(dataBackUp != null)
        {
            string msg = string.Format("Bạn đang ở tiến trình ({0}, wave {1}), bạn có muốn tiếp tục ?", dataBackUp.Mapname, dataBackUp.CurrentWave);
            GameManager.Instance.UIManager.ShowPopup(EPOPUP.COMMAND_POPUP, EMESSAGETYPE.MESSAGE, "CHÚ Ý", msg, () => {
                isContinueOldMap = true;

                //SceneManager.LoadScene(dataBackUp.Mapname);
                GameManager.Instance.UIManager.LoadScene(dataBackUp.Mapname, ESCREEN.IN_BATTLE);
            });
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
        if (!isContinueOldMap)
        {
            long goldInit = MapManager.GetStartingGold();
            int heartInit = MapManager.GetMainGateHealth();
            BattleManager = new BattleLogicManager(Mapname, goldInit, heartInit);
            UIManager.ShowScreen(ESCREEN.IN_BATTLE);
            //Debug.Log("BEGIN_NEW_BATTLE");
        }
        else
        {
            long goldInit = dataBackUp.CurrentGold;
            int heartInit = dataBackUp.CurrentHeart;
            BattleManager = new BattleLogicManager(Mapname, goldInit, heartInit);
            TurretManager.InitMapFromData(dataBackUp);
            UIManager.ShowScreen(ESCREEN.IN_BATTLE);
            //Debug.Log("CONTINUE_OLD_BATTLE");
        }
        
    }

    public void CreateWave()
    {
        if (BattleManager == null)
            return;
        BattleManager.CreateWave();
    }

    public void OnEnemyDie(long goldBonus)
    {
        BattleManager?.OnEnemyDie(goldBonus);
    }

    public void OnEnemyEscape()
    {
        BattleManager?.OnEnemyEscape();
    }

    /// <summary>
    /// Người chơi chủ động thoát trận đánh -> không lưu -> quay về menu
    /// </summary>
    public void PlayerReturnMenu()
    {
        //Remove All save data
        DataManager.Instance.RemoveAllDataSave();
        //Reset info battle in game
        MapManager = null;
        WaveManager = null;
        BattleManager = null;
        //Reset data ở enemymanager
        EnemyManager.RestartEnemyManager();
        //Reset data ở turretmanager
        TurretManager.ClearTurrets();

        //GameManager.Instance.UIManager.ShowScreen(ESCREEN.LOBBY);
        //SceneManager.LoadScene("MainMenu");
        GameManager.Instance.UIManager.LoadScene("MainMenu", ESCREEN.LOBBY);
    }

    public void OnPlayerRestartBattle()
    {
        dataBackUp = DataManager.Instance.GetDataSave();
        if (dataBackUp != null) {
            long goldInit = dataBackUp.CurrentGold;
            int heartInit = dataBackUp.CurrentHeart;
            BattleManager = new BattleLogicManager(dataBackUp.Mapname, goldInit, heartInit);
            TurretManager.InitMapFromData(dataBackUp);
            UIManager.ShowScreen(ESCREEN.IN_BATTLE);
        }
    }

    public bool IsCanBuildTurret(long turretCost, string turretName, out string errorMessage)
    {
        if (BattleManager == null)
        {
            errorMessage = "Không trong trận !";
            return false;
        }
        return BattleManager.IsCanBuildTurret(turretCost, turretName, out errorMessage);
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
        //print("LOADED SCENE " + scene.name);
        if (scene.name.Equals("MainMenu"))
        {
            GameManager.Instance.UIManager.ShowScreen(ESCREEN.LOBBY);
            return;
        }
        string mapname = scene.name;
        BeginNewBattle(mapname);
    }

    public void WriteDebug(string msg)
    {
        Debug.Log(msg);
    }
}


