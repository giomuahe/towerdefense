using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiUpgradeManager : MonoBehaviour
{
    public static UiUpgradeManager Instance;
    public Transform buttonParentPanel;
    public LoadPrefab loadPrefab = new LoadPrefab();
    public int id;
    public List<GameObject> listButton;
    public List<TurretType> listTurretNextLevel;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
       
    }


    void Update()
    {

    }

    public void SetID(int idTurret)
    {
        this.id = idTurret;
    }
    public void SetListTurret()
    {
        listTurretNextLevel = TurretManager.Instance.GetListTypeTurretToUpgradeById(this.id);
    }
    public void Clear(){
         foreach (Transform child in buttonParentPanel)
        {
            Destroy(child.gameObject);
        }
    }
    public void SpawnButton()
    {
        Clear();
        foreach (TurretType turretType in listTurretNextLevel)
        {
            GameObject newButtonPrefab = TurretManager.Instance.loadPrefab.LoadSelecTurretButton();
            GameObject newButton = Instantiate(newButtonPrefab, buttonParentPanel);
            TextMeshProUGUI buttonTextName = newButton.transform.Find("TextName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI buttonTextDescription = newButton.transform.Find("TextDescription").GetComponent<TextMeshProUGUI>();
            buttonTextName.text = TurretManager.Instance.TurretNameDictionNary()[turretType].TurretName;
            buttonTextDescription.text = TurretManager.Instance.TurretNameDictionNary()[turretType].TurretDescription;
            Debug.Log("button: " + TurretManager.Instance.TurretNameDictionNary()[turretType]);

            Button button = newButton.GetComponent<Button>();

            if (button != null)
            {
                SelectTurretButton param = newButton.GetComponent<SelectTurretButton>();
                param.SetParam(this.id, turretType);
                button.onClick.AddListener(() => OnButtonClicked(this.id, turretType));
            }
        }

    }
    void OnButtonClicked(int idTurret, TurretType turretType)
    {
        TurretManager.Instance.UpGradeTurret(idTurret, turretType);
        this.gameObject.SetActive(false);

    }
    void OnEnable()
    {

    }
}
