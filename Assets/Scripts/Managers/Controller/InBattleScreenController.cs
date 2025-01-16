using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InBattleScreenController : MonoBehaviour
{
    public PopupUpgradeController PopupUpgrade;
    
    public TextMeshProUGUI WaveTxt;
    public Text TimeText;
    public Text HeartText;
    public Text GoldText;
    public GameObject ButtonUpgrade;
    public int TurretBaseId = 1;

    private void OnEnable()
    {
        GameManager.Instance.BattleManager.OnWaveChanged += UpdateWaveInfo;
        GameManager.Instance.BattleManager.OnTimeChanged += UpdateTime;
        GameManager.Instance.BattleManager.OnGoldChanged += UpdateGold;
        GameManager.Instance.BattleManager.OnHeartChanged += UpdateHeart;
    }

    private void Start()
    {
        //Lấy giá trị khởi tạo
        GameManager.Instance.BattleManager.RefeshActionUI();
    }

    private void OnDisable()
    {
        GameManager.Instance.BattleManager.OnWaveChanged -= UpdateWaveInfo;
        GameManager.Instance.BattleManager.OnTimeChanged -= UpdateTime;
        GameManager.Instance.BattleManager.OnGoldChanged -= UpdateGold;
        GameManager.Instance.BattleManager.OnHeartChanged -= UpdateHeart;
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
    }

    private void UpdateGold(long currentGold){
        GoldText.text = currentGold.ToString("N0");
    }

    private void UpdateHeart(int currentHeart)
    {
        HeartText.text = currentHeart.ToString("N0");
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
        Debug.Log("ClickUpgrade");
        if (PopupUpgrade)
            PopupUpgrade.ShowPopupUpgrade(TurretBaseId);
    }

    public void OnClickNextWave(){
        GameManager.Instance.CreateWave();
    }

    public void OnClickPause()
    {
        bool isPause = GameManager.Instance.BattleManager.ChangePause();
        if (isPause)
        {

        }
    }
}
