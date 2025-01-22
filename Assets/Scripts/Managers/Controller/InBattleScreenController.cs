using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InBattleScreenController : MonoBehaviour
{
    public PopupUpgradeController PopupUpgrade;
    public PopupPauseController PopupPause;
    public ButtonPauseController ButtonPause;
    public PopupEndGameController PopupEndWinGame;
    public PopupEndGameController PopupEndLostGame;

    public TextMeshProUGUI WaveTxt;
    public Text TimeText;
    public Text HeartText;
    public Text GoldText;
    public GameObject ButtonUpgrade;
    public Button ButtonNextWave;
    public int TurretBaseId = 1;

    private void OnEnable()
    {
        //Debug.LogError("BATTLE_SCRREEN_ENABLE");

        GameManager.Instance.BattleManager.OnWaveChanged += UpdateWaveInfo;
        GameManager.Instance.BattleManager.OnTimeChanged += UpdateTime;
        GameManager.Instance.BattleManager.OnGoldChanged += UpdateGold;
        GameManager.Instance.BattleManager.OnHeartChanged += UpdateHeart;
        GameManager.Instance.BattleManager.EndGame += EndGame;

        ButtonNextWave.onClick.AddListener(OnClickNextWave);

        TimeText.text = "N/A";
        HeartText.text = "00";
        GoldText.text = "0.000";

        GameManager.Instance.BattleManager.RefeshActionUI();
    }

    private void Start()
    {
        //Lấy giá trị khởi tạo
        GameManager.Instance.BattleManager.RefeshActionUI();
    }

    private void OnDisable()
    {
        Debug.LogError("BATTLE_SCRREEN_DISABLE");
        if (GameManager.Instance.BattleManager != null)
        {
            GameManager.Instance.BattleManager.OnWaveChanged -= UpdateWaveInfo;
            GameManager.Instance.BattleManager.OnTimeChanged -= UpdateTime;
            GameManager.Instance.BattleManager.OnGoldChanged -= UpdateGold;
            GameManager.Instance.BattleManager.OnHeartChanged -= UpdateHeart;
            GameManager.Instance.BattleManager.EndGame -= EndGame;
        }

        //Ẩn các popup
        PopupUpgrade.ClosePopup();
        PopupPause.gameObject.SetActive(false);
        PopupEndWinGame.Hide();
        PopupEndLostGame.Hide();

        ButtonNextWave.onClick.RemoveAllListeners();
    }

    private void UpdateWaveInfo(int currentWave, int maxWave)
    {
        int currentWaveShow = currentWave + 1;
        WaveTxt.text = "<color=white>WAVE </color>" +
                       "<color=#FFC0CB>" + currentWaveShow.ToString("D2") + "</color>" + 
                       " / " +
                       "<color=yellow>" + maxWave.ToString("D2") + "</color>";
    }

    private void UpdateTime(int remainingTime)
    {
        int minutes = remainingTime / 60;
        int seconds = remainingTime % 60;
        TimeText.text = $"{minutes:00}:{seconds:00}";
        if(remainingTime <= 0)
        {
            ButtonNextWave.gameObject.SetActive(false);
        }
        else
        {
            ButtonNextWave.gameObject.SetActive(true);
        }
    }

    private void UpdateGold(long currentGold){
        GoldText.text = currentGold.ToString("N0");
    }

    private void UpdateHeart(int currentHeart)
    {
        HeartText.text = currentHeart.ToString("N0");
    }

    private void EndGame(bool isWin)
    {
        Debug.LogWarning("CALL_END_GAME " + isWin);
        if (isWin)
        {
            PopupEndWinGame.Show();
        }
        else
        {
            PopupEndLostGame.Show();
        }
    }

    public void ShowUpgradeButton(int turretBaseId)
    {
        TurretBaseId = turretBaseId;
        if (ButtonUpgrade)
            ButtonUpgrade.SetActive(true);
    }

    public void HideUpgradeButton()
    {
        TurretBaseId = 0;
        if (ButtonUpgrade)
            ButtonUpgrade.SetActive(false);
        if(PopupUpgrade)
            PopupUpgrade.ClosePopup();
    }

    /// <summary>
    /// ShowUpgrade UI
    /// </summary>
    public void OnClickUpgradeTurret()
    {
        if (PopupUpgrade)
            PopupUpgrade.ShowPopupUpgrade(TurretBaseId);
    }

    private void OnClickNextWave(){
        GameManager.Instance.CreateWave();
        ButtonNextWave.gameObject.SetActive(false);
    }

    public void OnClickPause(bool isPause)
    {
        PopupPause.gameObject.SetActive(isPause);
    }
}
