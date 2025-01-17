using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBullet : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    public float rotateSpeed = 200000f;
    public LayerMask enemyLayer;
    public int enemyLayerIndex;

    void Awake()
    {
        enemyLayer = LayerMask.GetMask("EnemyTarget");
        enemyLayerIndex=LayerMask.NameToLayer("EnemyTarget");

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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, speed* Time.deltaTime, LayerMask.GetMask("EnemyTarget")))
        {
            transform.position = Vector3.MoveTowards(transform.position, hit.point, speed * Time.deltaTime);
            if (hit.collider.gameObject.layer == enemyLayerIndex)
            {

                HitTarget(hit.collider.gameObject);
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
        EnemyDemo enemyDemo= enemy.GetComponent<EnemyDemo>();
        enemyDemo.TakeDamage(30f);
        // Destroy(gameObject);
    }
}
