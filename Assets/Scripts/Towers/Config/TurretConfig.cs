using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Turret Config", menuName = "NewTurretConfig")]
public class TurretConfig : ScriptableObject
{
    public string TurretName;
    public string TurretDescription;
    public float TurretHealth;
    public float AtkDamage;
    public float AtkSpeed;
    public float AtkRange;
    public float BulletSpeed;

    public TurretType TurretType;
    public int Cost;
    public string prefabName;
    public string bulletPrefabPath;
    public List<TurretType> UpgradeList;


}
public enum TurretType
{
    Base,
    Basic,
    Gatling_Level_1,
    Gatling_Level_2,
    Sniper_Level_1,
    Sniper_Level_2,
    Shotgun,
    Fire,
    Ice
}