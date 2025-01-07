using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Turret : MonoBehaviour
{
    public Transform aimTransform;
    public Transform target;
    public Transform firePos;
    public LayerMask enemyLayer;
    public Rig turretRig;
    public TurretConfig turretConfig;
    // turret Property
    public string TurretName;
    public string TurretDescription;
    public float TurretHealth;
    public float AtkDamage;
    public float AtkSpeed;
    public float AtkRange;
    public TurretType TurretType;
    public List<TurretType> UpgradeList;

    void Start()
    {
        target = null;
        turretRig = GetComponentInChildren<Rig>();
        LoadConfig();
    }
    void LoadConfig()
    {
        TurretName = turretConfig.TurretName;
        TurretDescription=turretConfig.TurretDescription;
        TurretHealth=turretConfig.TurretHealth;
        AtkDamage=turretConfig.AtkDamage;
        AtkSpeed=turretConfig.AtkSpeed;
        AtkRange=turretConfig.AtkRange;
        TurretType=turretConfig.TurretType;
        UpgradeList=turretConfig.UpgradeList;
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
            aimTransform.position = target.position;
            turretRig.weight = 1;
        }
        else
        {
            aimTransform.position = firePos.position;
            turretRig.weight = 0;
        }
    }
    void UpGrade(TurretType type){

    }
    void ButtonUpgradePress(){
        TuretManager.Instance.ShowUIUpgrade(UpgradeList);
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
        if (target != null)
        {
            return true;
        }
        return false;

    }


}
