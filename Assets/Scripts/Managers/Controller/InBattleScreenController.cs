using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InBattleScreenController : MonoBehaviour
{
    public PopupUpgradeController PopupUpgrade;
    public GameObject ButtonUpgrade;
    public int TurretBaseId = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUpgradeButton(int turretBaseId)
    {
        Debug.Log("OnClickShowUpgradeUI " + turretBaseId);
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
        GameManager.Instance.WaveNumericalOrder = 0;
        GameManager.Instance.CreateWave();
    }
}
