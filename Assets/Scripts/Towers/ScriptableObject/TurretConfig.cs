using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Turret Config", menuName= "NewTurretConfig")]
public class TurretConfig : ScriptableObject
{
    public string TurretName;
    public string TurretDescription;
   public float TurretHealth;
   public float AtkDamage;
   public float AtkSpeed;
   public float AtkRange;
   public TurretType TurretType;
   public List<TurretType> UpgradeList;
  
}
public enum TurretType{
    Basic,
    Gatling_Level_1,
    Gatling_Level_2,
    Sniper_Level_1,
    Sniper_Level_2,
    Shotgun,
    Fire,
    Ice
}