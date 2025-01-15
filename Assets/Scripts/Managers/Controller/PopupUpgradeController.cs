using MapConfigs;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class PopupUpgradeController : MonoBehaviour
{
    private int curretTurretIdSelect = 0;
    private TurretConfig turretUpgrade = null;

    public GameObject turretPrefab;
    public Transform contentParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPopupUpgrade(int curretTurretId)
    {
        curretTurretIdSelect = curretTurretId;
        //Lấy danh sách turret có thể upgrade
        List<TurretType> lsTurretUpgrade = GameManager.Instance.TurretManager.GetListTypeTurretToUpgradeById(curretTurretId);
        Dictionary<TurretType, TurretConfig> turretCfgs = GameManager.Instance.TurretManager.TurretNameDictionNary();

        // Xóa các phần tử cũ
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        //Init các phần tử mới
        foreach (var tur in lsTurretUpgrade) {
            TurretConfig turCfg = turretCfgs.GetValueOrDefault(tur);
            GameObject newItem = Instantiate(turretPrefab, contentParent);
            TurretInfoSelect turretInfo = newItem.GetComponent<TurretInfoSelect>();
            if (turretInfo)
            {
                turretInfo.SetInfo(turCfg.name, turCfg.TurretDescription);
            }
            Button btnSelect = newItem.GetComponent<Button>();
            if (btnSelect)
            {
                btnSelect.onClick.AddListener(() => OnTurretSelect(turCfg));
            }
        }
        this.gameObject.SetActive(true);
    }

    public void OnTurretSelect(TurretConfig cfg)
    {
        print("SELECT TURRET " + JsonConvert.SerializeObject(cfg));
        turretUpgrade = cfg;
    }

    public void ClosePopup()
    {
        curretTurretIdSelect = 0;
        turretUpgrade = null;
        this.gameObject.SetActive(false);
    }

    public void OnClickUpgrade()
    {
        print("CLICK_UPGRADE_TURRET " + curretTurretIdSelect + ", NextTurret = " + JsonConvert.SerializeObject(turretUpgrade));
        if (turretUpgrade)
            GameManager.Instance.TurretManager.UpGradeTurret(curretTurretIdSelect, turretUpgrade.TurretType);
        else
            GameManager.Instance.UIManager.ShowPopup(EPOPUP.CONFIRM_POPUP, "THÔNG BÁO", "Chưa chọn loại Turret để Upgrade !");
    }
}
