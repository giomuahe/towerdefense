using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTurretEnemy : EnemyBase
{
    protected float enemyTurretDamage;
    protected float attackRange;
    protected float attackSpeed;
    public override void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations)
    {
        attackRange = enemyConfig.AttackRange;
        attackSpeed = enemyConfig.AttackSpeed;
        enemyTurretDamage = enemyConfig.EnemyTurretDamage;
        base.SetUp(enemyConfig, moveLocations);
    }
}
