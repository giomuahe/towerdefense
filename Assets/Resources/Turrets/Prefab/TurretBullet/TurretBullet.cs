using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    public float rotateSpeed = 200000f;
    [SerializeField] protected float hitOffset = 0f;
    public LayerMask enemyLayer;
    public int enemyLayerIndex;
    [SerializeField] protected GameObject hit;
    [SerializeField] public GameObject flash;
    [SerializeField] protected ParticleSystem hitPS;
    [SerializeField] protected Light lightSourse;
    [SerializeField] protected ParticleSystem projectilePS;
    [SerializeField] protected Vector3 rotationOffset = new Vector3(0, 0, 0);
    [SerializeField] protected GameObject[] Detached;
    private bool startChecker = false;
    [SerializeField] protected bool notDestroy = false;
    [SerializeField] protected bool UseFirePointRotation;
    [SerializeField] protected Collider col;

    void Awake()
    {
        enemyLayer = LayerMask.GetMask("EnemyTarget");
        enemyLayerIndex = LayerMask.NameToLayer("EnemyTarget");

    }
    protected void Start()
    {
        if (!startChecker)
        {
            /*lightSourse = GetComponent<Light>();
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
    public void SetTarget(Transform curTarget)
    {

        this.target = curTarget;

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
        Vector3 direction = (target.position - transform.position).normalized;

        if (lightSourse != null){
            lightSourse.enabled = false;
            projectilePS.Stop();
        projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);}
        col.enabled = false;
        
        RaycastHit raycastHit;

        if (Physics.Raycast(transform.position, direction, out raycastHit, speed * Time.deltaTime, LayerMask.GetMask("Enemy")))
        {

            transform.position = Vector3.MoveTowards(transform.position, raycastHit.point, speed * Time.deltaTime);

            if (raycastHit.collider.gameObject.transform == target)
            {
                
                ParticleSystem hitEffect = hitPS;
                ParticleSystem effect = Instantiate(hitEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
                // hitEffect.Play();
                Destroy(effect.gameObject, 0.5f);
                HitTarget(raycastHit.collider.gameObject);
            }
        }
        Quaternion toRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);

        // Di chuyển về phía Enemy
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    void HitTarget(GameObject enemy)
    {
        Debug.Log("Đã trúng");
        EnemyBase enemyTakedame= enemy.GetComponent<EnemyBase>();
        int bulletDamge = 1;
        bool isEnemyDie;
        GameManager.Instance.EnemyManager.SendDamage(enemyTakedame.EnemyID(), bulletDamge, out isEnemyDie);
        if(isEnemyDie){
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject);
    }
    void HitEffect()
    {

    }
}
