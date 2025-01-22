using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.DATA;
using MapConfigs;
using Newtonsoft.Json;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TurretManager : MonoBehaviour
{

    public Dictionary<int, GameObject> TurretOnMapDic;
    public LoadPrefab loadPrefab;
    public List<TurretConfig> turretConfigs;
    //Lay tu MapManager
    //Lay tu MapManager
    void Awake()
    {
        loadPrefab = new LoadPrefab();
        TurretOnMapDic = new Dictionary<int, GameObject>();

    }

    public void ClearTurrets()
    {
        TurretOnMapDic.Clear();
    }
    void Start()
    {

    }
    void BuildTurret(int id, TurretType turretType)
    {
        print("TURRET BUILD id = " + id + ", type = " + turretType);
        if (TurretOnMapDic.ContainsKey(id))
        {
            Destroy(TurretOnMapDic[id]);
        }
        GameObject gameObject = loadPrefab.LoadTurret(turretType);
        TurretBase turret = GameManager.Instance.MapManager.GetTurretBase(id);
        //print("TURRET BUILD " + JsonConvert.SerializeObject(turret));
        GameObject newTurret = Instantiate(gameObject, turret.Position, Quaternion.identity);
        Turret newTurretClass = newTurret.GetComponent<Turret>();
        newTurretClass.SetID(id);

        TurretOnMapDic[id] = newTurret;
        GameManager.Instance.MapManager.UpdateTurret(id, newTurret, turretType);
        
        GameManager.Instance.UIManager.SpawnHealthBarUI(newTurret.transform);
    }
    void Update()
    {

    }
    public void UpGradeTurret(int id, TurretType newTurret)
    {
        BuildTurret(id, newTurret);
    }
    public List<TurretType> GetListTypeTurretToUpgradeById(int id)
    {
        if (TurretOnMapDic.ContainsKey(id))
        {
            GameObject turret = TurretOnMapDic[id];
            Turret currentturret = turret.GetComponent<Turret>();
            return currentturret.GetListTypeTurretUpgrade();
        }
        else
        {
            return new List<TurretType>() { TurretType.Basic };
        }

    }
    public Dictionary<TurretType, TurretConfig> TurretInfoDictionNary()
    {
        Dictionary<TurretType, TurretConfig> tDic = new Dictionary<TurretType, TurretConfig>();
        foreach (TurretConfig trconfig in turretConfigs)
        {
            tDic[trconfig.TurretType] = trconfig;
        }
        return tDic;
    }
    public void SendDamage(int id, float damage)
    {
        TurretOnMapDic[id].GetComponent<Turret>().TakeDamage(damage);
    }
    public void RemoveTurret(int id)
    {
        Destroy(TurretOnMapDic[id]);
        GameManager.Instance.MapManager.UpdateTurret(id, null, TurretType.Base);
    }

    /// <summary>
    /// Xây turret từ thông tin đã có
    /// </summary>
    /// <param name="data"></param>
    public void InitMapFromData(SaveData data)
    {
        Debug.Log("BUILD_MAP " + JsonConvert.SerializeObject(data));
        Dictionary<int, TurretData> turretInfo = data.TurretInfo;
        //Xây turret trên map
        foreach (KeyValuePair<int, TurretData> idAndTurretData in turretInfo)
        {
            if (idAndTurretData.Value.TurretType != TurretType.Base)
                BuildTurret(idAndTurretData.Key, idAndTurretData.Value.TurretType);
        }

        //Update thông tin ở MapManager.UpdateTurret()
    }

}
