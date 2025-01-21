using Assets.Scripts.Enums;
using MapConfigs;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
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
        if(lsTurretUpgrade == null || lsTurretUpgrade.Count == 0){
            //TODO
            GameManager.Instance.UIManager.ShowPopup(EPOPUP.CONFIRM_POPUP, EMESSAGETYPE.MESSAGE,"THÔNG BÁO", "Trụ đã đạt cấp tối đa !");
            return;
        }
        Dictionary<TurretType, TurretConfig> turretCfgs = GameManager.Instance.TurretManager.TurretInfoDictionNary();

        // Xóa các phần tử cũ
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        //Init các phần tử mới
        foreach (var tur in lsTurretUpgrade) {
            TurretConfig turCfg = turretCfgs.GetValueOrDefault(tur);
            if(turCfg == null){
                GameManager.Instance.UIManager.ShowPopup(EPOPUP.CONFIRM_POPUP, EMESSAGETYPE.MESSAGE, "THÔNG BÁO", "Không tìm được thông tin trụ " + tur);
                Debug.LogError("TUREET CONFIG OF " + tur);
                return;
            }
            GameObject newItem = Instantiate(turretPrefab, contentParent);
            TurretInfoSelect turretInfo = newItem.GetComponent<TurretInfoSelect>();
            if (turretInfo)
            {
                Debug.Log("UPGRADE TURRET " + turCfg);
                turretInfo.SetInfo(turCfg.TurretName, turCfg.TurretDescription, turCfg.Cost);
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
        {
            //Trừ tiền của người chơi
            long turretCost = turretUpgrade.Cost;
            string turretName = turretUpgrade.TurretName;
            string errorMess = "Unknow_error";
            bool isCanBuildTurret = GameManager.Instance.IsCanBuildTurret(turretCost, turretName, out errorMess);
            if (isCanBuildTurret)
            {
                GameManager.Instance.TurretManager.UpGradeTurret(curretTurretIdSelect, turretUpgrade.TurretType);
                turretUpgrade = null;
                this.gameObject.SetActive(false);
            }
            else
            {
                GameManager.Instance.UIManager.ShowPopup(EPOPUP.CONFIRM_POPUP, EMESSAGETYPE.ERROR, "THÔNG BÁO", errorMess);
            }
            
        }
        else
            GameManager.Instance.UIManager.ShowPopup(EPOPUP.CONFIRM_POPUP, EMESSAGETYPE.WARNING, "THÔNG BÁO", "Chưa chọn loại Turret để Upgrade !");
    }
}
