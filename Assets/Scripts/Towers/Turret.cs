using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Turret : MonoBehaviour, IHealthBar
{
    public int id;
    [SerializeField]
    protected Transform aimTransform;
    [SerializeField]
    protected GameObject target;
    [SerializeField]
    protected Transform firePos;

    protected Rig turretRig;
    [SerializeField]
    protected TurretConfig turretConfig;
    // turret Property
    protected string TurretName;
    protected string TurretDescription;
    protected float TurretHealth;
    protected float AtkDamage;
    protected float AtkSpeed;
    protected float AtkRange;
    protected float AtkAngle;
    protected float BulletSpeed;
    protected string BulletPath;
    protected TurretType TurretType;
    protected List<TurretType> UpgradeList;


    public float currentHp;

    protected void Start()
    {

        turretRig = GetComponentInChildren<Rig>();
        Initialize();
    }

    public void LoadConfig()
    {
        TurretName = turretConfig.TurretName;
        TurretDescription = turretConfig.TurretDescription;
        TurretHealth = turretConfig.TurretHealth;
        AtkDamage = turretConfig.AtkDamage;
        AtkSpeed = turretConfig.AtkSpeed;
        AtkRange = turretConfig.AtkRange;
        TurretType = turretConfig.TurretType;
        UpgradeList = turretConfig.UpgradeList;
        BulletSpeed = turretConfig.BulletSpeed;
        BulletPath = turretConfig.bulletPrefabPath;
        AtkAngle= turretConfig.AtkAngle;
    }

    void Update()
    {
        UpdateTarget();
        FindEnemy();
        if (firePos != null)
        {
            Aim();
        }
        if(attackCooldown<0){
            attackCooldown=0;
        }
        UpdateAtkState();
    }
    public virtual void UpdateAtkState(){

    }

    public void SetID(int currentId)
    {
        this.id = currentId;
    }
    public void UpdateTarget()
    {
        if (LostEnemy())
        {
            target = null;
        }
    }
    bool LostEnemy()
    {
        if (target == null) return false;
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
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
        if (CanFindEnemy())
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, AtkRange, enemyTargetLayermark);
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (var hit in hits)
            {
                float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
                if (distanceToTarget < shortestDistance)
                {
                    shortestDistance = distanceToTarget;
                    nearestEnemy = hit.gameObject;
                }
            }
            target = nearestEnemy;
        }
        else
        {
            return;
        }
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
    void Aim()
    {
        if (CanAim())
        {

            aimTransform.position = target.transform.position;
            turretRig.weight = 1;
            Vector3 directionToTarget = (target.transform.position - firePos.position).normalized;

            // Tính góc giữa hướng nòng súng và hướng đến mục tiêu
            float angle = Vector3.Angle(firePos.forward, directionToTarget);
            if (angle < 5f){
                TryToAttack();
                isAttacking=true;
            }
        }
        else
        {  isAttacking=false;
            aimTransform.position = firePos.position;
            turretRig.weight = 0;
        }
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
        //Debug.Log("");
        if (CanTakeDamage())
        {
            currentHp = currentHp - damage;
            if (currentHp <= 0)
            {
                Die();
                return;
            }
            var healthCheck = currentHp / TurretHealth;
            OnHealthChange.Invoke(healthCheck);
        }
        else
        {
            return;
        }

    }
    public bool CanTakeDamage()
    {
        if (currentHp <= 0)
        {
            return false;
        }
        if (currentHp > 0)
        {
            return true;
        }
        return false;
    }
    public void Die()
    {
        GameManager.Instance.TurretManager.RemoveTurret(this.id);

    }
    // public int bulletID = 102;
    // public ObjectPooling objectPooling;
    // void Starts(){
    //     objectPooling= GameManager.Instance.PoolManager.GetPoolThroughID(bulletID);

    // }
    // public void Attack(){
    //     objectPooling.SetPosition(firePos);
    //     objectPooling.Pool.Get();
    // }
    public LayerMask enemyTargetLayermark;
public bool isAttacking;
    public void Attack()
    {
        Debug.Log("shoot");
        Fire();
    }
    public float attackCooldown;
    private void TryToAttack()
    {
        if (target != null && attackCooldown == 0f)
        {
            Attack();
            attackCooldown = 1f / AtkSpeed;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
            

        }
    }

    public virtual void Initialize()
    {
        target = null;
        LoadConfig();
        enemyTargetLayermark = LayerMask.GetMask("Enemy");
        currentHp = TurretHealth;
        attackCooldown = 0;
        
    }

    public GameObject bulletPrefab;

    public event Action<float> OnHealthChange;

    public virtual void Fire()
    {

    }
    public List<TurretType> GetListTypeTurretUpgrade()
    {
        return UpgradeList;
    }

}
