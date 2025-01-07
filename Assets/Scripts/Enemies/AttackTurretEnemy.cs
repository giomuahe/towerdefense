using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTurretEnemy : EnemyBase
{
    protected float enemyTurretDamage;
    protected float attackRange;
    protected float attackSpeed;
    protected float bulletSpeed;
    [SerializeField]
    private LayerMask turretLayerMask;
    [SerializeField]
    private Transform shootPos;
    public override void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations, int enemyInGameID)
    {
        attackRange = enemyConfig.AttackRange;
        attackSpeed = enemyConfig.AttackSpeed;
        enemyTurretDamage = enemyConfig.EnemyTurretDamage;
        base.SetUp(enemyConfig, moveLocations, enemyInGameID);
    }

    IEnumerator Attack(int turretID)
    {
        yield return new WaitForSeconds(attackRange/bulletSpeed);
        GameManager.Instance.NotifyOnTurretHit(turretID);
    }
}
