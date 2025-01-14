using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InBattleScreenController : MonoBehaviour
{
    public GameObject PopupUpgrade;
    public Button UpgradeBtn;
    public int TurretBaseId = 1;
    public TurretInfoSelect TurretSelect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        UpgradeBtn.onClick.AddListener(OnClickUpgradeTurret);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickShowUpgradeUI()
    {
        if(PopupUpgrade)
            PopupUpgrade.SetActive(true);
        //Info
        //GameManager.Instance.TurretManager.TurretInfoDictionNary();


    }

    public void OnClickHideUpgradeUI()
    {
        if (PopupUpgrade)
            PopupUpgrade.SetActive(false);
    }

    public void OnClickUpgradeTurret()
    {
        TurretType type = TurretType.Basic;
        Debug.Log("ClickUpgrade Turret Id = " + TurretBaseId + ", type = " + type);
        GameManager.Instance.TurretManager.UpGradeTurret(TurretBaseId, type);
    }
}
