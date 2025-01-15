using System;
using System.Collections;
using System.Collections.Generic;
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
  
    void Start()
    {
     
        

    }
    void BuildTurret(int id, TurretType turretType)
    {
        if (TurretOnMapDic[id] != null)
        {
            Destroy(TurretOnMapDic[id]);
        }
        GameObject gameObject = loadPrefab.LoadTurret(turretType);
        GameObject newTurret = Instantiate(gameObject, GameManager.Instance.MapManager.GetTurretBase(id).Position, Quaternion.identity);
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
        GameObject turret= TurretOnMapDic[id];
        Turret currentturret= turret.GetComponent<Turret>();
        return currentturret.UpgradeList;
    }
    public Dictionary<TurretType, TurretConfig> TurretInfoDictionNary(){
        Dictionary<TurretType, TurretConfig> tDic= new Dictionary<TurretType, TurretConfig>();
        foreach(TurretConfig trconfig in turretConfigs){
            tDic[trconfig.TurretType]= trconfig;
        }
        return tDic;
    }
    public void SendDamage(int id, float damage){
        TurretOnMapDic[id].GetComponent<Turret>().TakeDamage(damage);
    }
   
}
