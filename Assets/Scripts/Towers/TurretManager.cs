using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance;
    public Dictionary<int, GameObject> TurretOnMapDic;
    public LoadPrefab loadPrefab;
    public List<TurretConfig> turretConfigs;
    
    //Lay tu MapManager
    public List<Transform> TurretTransformsList;
    public Dictionary<int, Transform> TurretTransformDic;


    Dictionary<int, Transform> InitializeDictionaryTurretTransform()
    {
        Dictionary<int, Transform> dictionary = new Dictionary<int, Transform>();
        foreach (Transform transform in TurretTransformsList)
        {
            dictionary.Add(TurretTransformsList.IndexOf(transform), transform);
        }
        return dictionary;

    }
    //Lay tu MapManager
    void Awake()
    {
        Instance = this;
        loadPrefab = new LoadPrefab();
        TurretOnMapDic = new Dictionary<int, GameObject>();
        UiUpgradeManager.Instance.gameObject.SetActive(false);

    }
    void InitializeStartGame()
    {
        TurretTransformDic = InitializeDictionaryTurretTransform();
        foreach (int id in TurretTransformDic.Keys)
        {
            TurretOnMapDic[id] = null;
           BuildTurret(id, TurretType.Base);
        }

    }
    void Start()
    {
        InitializeStartGame();
        

    }

    void BuildTurret(int id, TurretType turretType)
    {
        if (TurretOnMapDic[id] != null)
        {
            Destroy(TurretOnMapDic[id]);
        }
        GameObject gameObject = loadPrefab.LoadTurret(turretType);
        GameObject newTurret = Instantiate(gameObject, TurretTransformDic[id].position, Quaternion.identity);
        Turret newTurretClass=newTurret.GetComponent<Turret>();
        newTurretClass.SetID(id);

        TurretOnMapDic[id] = newTurret;
    }
    void Update()
    {

    }
    public void UpGradeTurret(int id, TurretType newTurret)
    {
        BuildTurret(id, newTurret);
    }
    public List<TurretType> GetListTypeTurretToUpgradeById(int id){
        if (TurretOnMapDic.ContainsKey(id))
        {
            GameObject turret = TurretOnMapDic[id];
            Turret currentturret = turret.GetComponent<Turret>();
            return currentturret.UpgradeList;
        }
        else
        {
            return new List<TurretType>(){ TurretType.Basic};
        }
        
    }
    public Dictionary<TurretType, TurretConfig> TurretNameDictionNary(){
        Dictionary<TurretType, TurretConfig> tDic= new Dictionary<TurretType, TurretConfig>();
        foreach(TurretConfig trconfig in turretConfigs){
            tDic[trconfig.TurretType]= trconfig;
        }
        return tDic;
    }
    public void SendDamage(int id){
        TurretOnMapDic[id].GetComponent<Turret>().TakeDamage(200);
    }
    public void ShowUIUpgrade(int id)
    {
        UiUpgradeManager.Instance.gameObject.SetActive(true);
        UiUpgradeManager.Instance.SetID(id);
        UiUpgradeManager.Instance. SetListTurret();
        UiUpgradeManager.Instance.SpawnButton();


    }
}
