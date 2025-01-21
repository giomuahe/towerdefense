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

        RaycastHit raycastHit;

        if (Physics.Raycast(transform.position, direction, out raycastHit, speed * Time.deltaTime, enemyLayer))
        {

            transform.position = Vector3.MoveTowards(transform.position, raycastHit.point, speed * Time.deltaTime);
            if (target != null)
            {
                // EnemyDemo enemy = raycastHit.collider.GetComponent<EnemyDemo>();

                // if (enemy.id == target.GetComponent<EnemyDemo>().id)
                // {
                //     ParticleSystem hitEffect = hitPS;
                //     ParticleSystem effect = Instantiate(hitEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
                //     // hitEffect.Play();

                //     Debug.Log("enemyId:" + enemy.id);
                //     Destroy(effect.gameObject, 0.5f);

                //     HitTarget(enemy);

                // }
                EnemyBase enemy = raycastHit.collider.gameObject.GetComponent<EnemyBase>();
                if (enemy.GetEnemyInGameID() == target.GetComponent<EnemyBase>().GetEnemyInGameID())
                {
                    ParticleSystem hitEffect = hitPS;
                    ParticleSystem effect = Instantiate(hitEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
                    // hitEffect.Play();
                    Debug.Log("enemyId:" + enemy.GetEnemyInGameID());
                    Destroy(effect.gameObject, 0.5f);
                    HitTarget(enemy);

                }
            }
            else
            {
                Destroy(gameObject);
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


        // enemy.TakeDamage(damage);
        // Debug.Log("enemyId:" + enemy.id);

        Destroy(this.gameObject);
    }
    void HitEffect()
    {

    }

}
