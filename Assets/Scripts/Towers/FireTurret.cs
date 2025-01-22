using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireTurret : Turret
{
    private const string LINK_TURRET_BULLET_PREFAB = "Turrets/TurretBullet/";
    private Dictionary<EnemyDemo, GameObject> burningEnemies = new Dictionary<EnemyDemo, GameObject>();
    public float burnDuration = 3f;

    public override void Initialize()
    {
        base.Initialize();

        bulletPrefab = Resources.Load<GameObject>(LINK_TURRET_BULLET_PREFAB + turretConfig.bulletPrefabPath);


    }
    public override void Fire()
    {
        base.Fire();
        CheckEnemyInCone();

    }
    void CheckEnemyInCone()
    {
        Collider[] hits = Physics.OverlapSphere(firePos.position, AtkRange, enemyTargetLayermark);
        List<EnemyDemo> enemiesInRange = new List<EnemyDemo>();
        foreach (Collider hit in hits)
        {
            EnemyDemo enemyDemo = hit.GetComponent<EnemyDemo>();
            if (enemyDemo != null && IsInCome(enemyDemo.transform))
            {
                enemiesInRange.Add(enemyDemo);
                if (!burningEnemies.ContainsKey(enemyDemo))
                {
                    StartBurning(enemyDemo);
                }
            }
        }
        // Kiểm tra enemy nào ra khỏi vùng bắn
        List<EnemyDemo> toRemove = new List<EnemyDemo>();
        foreach (var enemy in burningEnemies.Keys)
        {
            if (!enemiesInRange.Contains(enemy))
            {
                StopBurning(enemy);
                toRemove.Add(enemy);
            }
        }

        // Xóa enemy đã ra khỏi vùng bắn
        foreach (var enemy in toRemove)
        {
            burningEnemies.Remove(enemy);
        }
    }
    bool IsInCome(Transform enemy)
    {
        Vector3 directtiontoTarget = (enemy.position - firePos.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directtiontoTarget);
        return angleToTarget <= AtkAngle / 2;
    }
    void StartBurning(EnemyDemo enemyDemo)
    {
        GameObject fireEffect = Instantiate(bulletPrefab, enemyDemo.transform.position, Quaternion.identity, enemyDemo.transform);
        burningEnemies[enemyDemo] = fireEffect;
        StartCoroutine(StopBurnAfterDuration(enemyDemo, burnDuration));
    }
    void StopBurning(EnemyDemo enemy)
    {
        if (burningEnemies.ContainsKey(enemy))
        {
            StartCoroutine(StopBurnAfterDuration(enemy, burnDuration));
        }
    }
    IEnumerator StopBurnAfterDuration(EnemyDemo enemy, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (burningEnemies.ContainsKey(enemy))
        {
            Destroy(burningEnemies[enemy]);
            burningEnemies.Remove(enemy);
        }
    }

}
