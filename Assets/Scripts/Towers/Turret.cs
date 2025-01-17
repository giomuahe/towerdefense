using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Turret : MonoBehaviour
{
    public int id;
    public Transform aimTransform;
    public Transform target;
    public Transform firePos;
    
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
    
    public void SetTurretConfig(TurretConfig config)
    {
        this.turretConfig = config;
    }

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
    }

    void Update()
    {
        UpdateTarget();
        FindEnemy();

        Aim();
    }

    public void SetID(int currentId)
    {
        this.id = currentId;
    }
    public void UpdateTarget()
    {
        if (LostEnemy())
        {
            target.gameObject.layer = LayerMask.NameToLayer("Enemy");
            target = null;
        }
    }
    bool LostEnemy()
    { 
        if (target == null) return false;
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
        if (CanFindEnemy())
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, AtkRange, LayerMask.GetMask("Enemy"));
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
            target.gameObject.layer = LayerMask.NameToLayer("EnemyTarget"); // Đổi Layer khi bị Aim
            aimTransform.position = target.position;
            turretRig.weight = 1;
            TryToAttack();
        }
        else
        {
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
        if (CanTakeDamage())
        {
            currentHp = currentHp - damage;
            if (currentHp <= 0)
            {
                Die();
                return;
            }
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
        TurretMain.SetActive(false);

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
    public LayerMask enemyOriginLayermark;
    public void Attack()
    {

        RaycastHit hit;
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, AtkRange, enemyTargetLayermark))
        {
            if (hit.transform == target)
            {
                Debug.Log("shoot");
                Fire();

            }
        }

    }
    private float attackCooldown;
    private void TryToAttack()
    {
        if (target != null && attackCooldown <= 0f)
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
        enemyTargetLayermark = LayerMask.GetMask("EnemyTarget");
        enemyOriginLayermark = LayerMask.GetMask("EnemyTarget");
        currentHp = TurretHealth;
        attackCooldown = 0;
    }

    public GameObject bulletPrefab;
    public virtual void Fire()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);
        TurretBullet turretBullet = bulletObj.GetComponent<TurretBullet>();
        turretBullet.SetTarget(target);
    }


}
