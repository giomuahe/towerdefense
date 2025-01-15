using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Turret : MonoBehaviour
{
    public int id;
    public Transform aimTransform;
    public Transform target;
    public Transform firePos;
    public LayerMask enemyLayer;
    public Rig turretRig;
    public TurretConfig turretConfig;
    public GameObject TurretMain;
    // turret Property
    public string TurretName;
    public string TurretDescription;
    public float TurretHealth;
    public float AtkDamage;
    public float AtkSpeed;
    public float AtkRange;
    public TurretType TurretType;
    public List<TurretType> UpgradeList;

    public float currentHp;
    float nextAttackTime = 0;

    void Start()
    {
        target = null;
        turretRig = GetComponentInChildren<Rig>();
        LoadConfig();
        currentHp = TurretHealth;
        Starts();
    }
    void LoadConfig()
    {
        TurretName = turretConfig.TurretName;
        TurretDescription = turretConfig.TurretDescription;
        TurretHealth = turretConfig.TurretHealth;
        AtkDamage = turretConfig.AtkDamage;
        AtkSpeed = turretConfig.AtkSpeed;
        AtkRange = turretConfig.AtkRange;
        TurretType = turretConfig.TurretType;
        UpgradeList = turretConfig.UpgradeList;
    }

    void Update()
    {
        if (CanUpdateEnemy())
        {
            target = null;
        }
        if (CanFindEnemy())
        {
            FindEnemy();
        }
        if (CanAim())
        {
            if (TurretType == TurretType.Base)
            {
                return;
            }
            aimTransform.position = target.position;
            turretRig.weight = 1;
            //InvokeRepeating("Attack", 1, 1);
            if(Time.realtimeSinceStartup > nextAttackTime)
                Attack();
        }
        else
        {
            if (TurretType == TurretType.Base)
            {
                return;
            }
            aimTransform.position = firePos.position;
            turretRig.weight = 0;
        }
    }
    void UpGrade(TurretType type)
    {

    }
   
    public void SetID(int currentId)
    {
        this.id = currentId;
    }
    bool CanUpdateEnemy()
    {
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget > AtkRange)
            {
                return true;
            }
            return false;
        }
        return false;
    }
    void FindEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, AtkRange, enemyLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;
        foreach (var hit in hits)
        {
            float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
            if (distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                nearestEnemy = hit.transform;
            }
        }
        target = nearestEnemy;
    }

    public bool CanFindEnemy()
    {
        if (target != null)
        {
            return false;
        }
        if (target == null)
        {
            return true;
        }
        return false;
    }
    public bool CanAim()
    {
        if (target == null)
        {
            return false;
        }
        if (TurretType == TurretType.Base)
        {
            return false;
        }
        if (firePos == null)
        {
            return false;
        }
        if (target != null)
        {
            return true;
        }
        return false;

    }
    public void TakeDamage(float damage)
    {
        if(CanTakeDamage()){
        currentHp = currentHp - damage;
        if (currentHp <= 0)
        {
            Die();
            return;
        }}else{
            return;
        }

    }
    public bool CanTakeDamage(){
        if(currentHp<=0){
            return false;
        }
        if(currentHp>0){
            return true;
        }
        return false;
    }
    public void Die()
    {
        TurretMain.SetActive(false);
    
    }
    public GameObject bullet;
    public int bulletID = 102;
    public ObjectPooling objectPooling;
    void Starts(){
        objectPooling= GameManager.Instance.PoolManager.GetPoolThroughID(bulletID);
        
    }
    public void Attack(){
        nextAttackTime = Time.realtimeSinceStartup + 1;
        //
        objectPooling.SetPosition(firePos);
        //Debug.Log("ATTACK 1");
        objectPooling.Pool.Get();
        //Debug.Log("ATTACK 2");
    }


}
