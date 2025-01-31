using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private GameObject target;

    public float rotateSpeed = 200000f;
    public LayerMask enemyLayer;
    public int enemyLayerIndex;
    [SerializeField] protected GameObject hit;
    [SerializeField] protected GameObject flash;
    [SerializeField] protected ParticleSystem hitPS;
    [SerializeField] protected Light lightSourse;
    [SerializeField] protected ParticleSystem projectilePS;
    [SerializeField] protected Vector3 rotationOffset = new Vector3(0, 0, 0);
    [SerializeField] protected GameObject[] Detached;
    private bool startChecker = false;
    [SerializeField] protected bool notDestroy = false;
    [SerializeField] protected bool UseFirePointRotation;
    [SerializeField] protected Collider col;

    // bullet property;
    [SerializeField] private float speed;
    [SerializeField] private int damage;

    void Awake()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        col = GetComponent<Collider>();
        lightSourse = GetComponent<Light>();
        // Transform projectile = transform.Find("Projectile");
        // projectilePS= projectile.GetComponent<ParticleSystem>();

    }
    protected void Start()
    {
        
        if (!startChecker)
        {
            /*
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            if (hit != null)
                hitPS = hit.GetComponent<ParticleSystem>();*/
            // if (flash != null)
            // {
            //     flash.transform.parent = null;
            // }
        }
        if (notDestroy)
            StartCoroutine(DisableTimer(5));
        else
            Destroy(gameObject, 5);
        startChecker = true;
    }
    protected IEnumerator DisableTimer(float time)
    {
        yield return new WaitForSeconds(time);
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        yield break;
    }
    protected virtual void OnEnable()
    {
        if (startChecker)
        {
            if (flash != null)
            {
                flash.transform.parent = null;
            }
            if (lightSourse != null)
                lightSourse.enabled = true;
            col.enabled = true;
        }
    }
    public void SetTarget(GameObject curTarget)
    {

        this.target = curTarget;

    }
    public void SetSpeed(float bulletSpeed)
    {
        this.speed = bulletSpeed;

    }
    public void SetDamage(float turretDamage)
    {
        this.damage = Mathf.RoundToInt(turretDamage);
    }
    public GameObject GetFlash()
    {
        return this.flash;
    }


    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        ChaseTarget();
    }
    void ChaseTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;

        if (lightSourse != null)
        {
            projectilePS.Stop();
            projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        col.enabled = false;

        RaycastHit[] allEnemy = Physics.RaycastAll(transform.position, direction, speed * Time.deltaTime, enemyLayer);
        if(allEnemy.Length > 0)
        {

        }

        RaycastHit raycastHit;
        int targetId = target.GetComponent<EnemyBase>().GetEnemyInGameID();
       

        if (Physics.Raycast(transform.position, direction, out raycastHit, speed * Time.deltaTime, enemyLayer))
        {
             transform.position = Vector3.MoveTowards(transform.position, raycastHit.point, speed * Time.deltaTime);
            RaycastHit enemyAttack;
            RaycastHit[] allEnemyBeSide = Physics.SphereCastAll(raycastHit.point, 2, direction, enemyLayer);
            foreach(var ray in allEnemyBeSide)
             {
                EnemyBase enemyBase = ray.collider.GetComponent<EnemyBase>();
                if((enemyBase != null) && (enemyBase.GetEnemyInGameID() == targetId)){
                    enemyAttack = ray;
                    ParticleSystem hitEffect = hitPS;
                    ParticleSystem effect = Instantiate(hitEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
                    Debug.Log("enemyId:" + enemyBase.GetEnemyInGameID());
                    Destroy(effect.gameObject, 0.5f);
                    HitTarget(enemyBase);
                }
                else
                {
                    Destroy(gameObject);
                }
               
            }
           
            
        }
        Quaternion toRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);

        // Di chuyển về phía Enemy
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    void HitTarget(EnemyBase enemy)
    {
        Debug.Log("Đã trúng");
      
        bool isEnemyDie;
        GameManager.Instance.EnemyManager.SendDamage(enemy.EnemyID(), damage, out isEnemyDie);

        Destroy(this.gameObject);
    }
   
    void HitEffect()
    {

    }

}
