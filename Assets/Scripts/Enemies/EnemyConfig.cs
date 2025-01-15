using MapConfigs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyConfig : ScriptableObject
{
    [Header("Stats Setting")]
    public float EnemyHealth;
    public float EnemySpeed;
    public float AttackSpeed;
    public float EnemyNexusDamage;
    public float EnemyTurretDamage;
    public float AttackRange;
    public int GoldDropAmount;
    public int BulletID;
    [Header("Others")]
    public EnemyType EnemyType;
    public GameObject EnemyPrefab;
}
