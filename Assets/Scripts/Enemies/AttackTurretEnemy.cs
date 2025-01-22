using Managers;
using MapConfigs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class AttackTurretEnemy : EnemyBase
{
    protected float enemyTurretDamage;
    protected float attackRange;
    protected float attackSpeed;
    protected float bulletSpeed;
    protected int baseTurretID;
    private Transform turretTarget;

    protected StateMachine enemyStateMachine;
    private StateMachine.State moveState;
    protected StateMachine.State attackState;
    private StateMachine.State deadState;
    private bool isTargetBeingDestroy;

    private int bulletID;
    private ObjectPooling bulletPool;

    [SerializeField]
    private Transform shootPos;

    [SerializeField]
    private float rotationSpeed;

    private float attackTimer;

    private float rotationTimer;

    public override void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations, int enemyInGameID)
    {
        attackRange = enemyConfig.AttackRange;
        attackSpeed = enemyConfig.AttackSpeed;
        bulletSpeed = enemyConfig.BulletSpeed;
        enemyTurretDamage = enemyConfig.EnemyTurretDamage;
        isTargetBeingDestroy = false;
        base.SetUp(enemyConfig, moveLocations, enemyInGameID);
        bulletID = enemyConfig.BulletID;
        bulletPool = GameManager.Instance.PoolManager.GetPoolThroughID(bulletID);
        enemyStateMachine = new StateMachine();
        CreateState();
    }

    protected virtual void CreateState()
    {
        moveState = enemyStateMachine.CreateState("move");
        moveState.onEnter = delegate
        {
            Move(moveLocations[locationIndex]);
            Dictionary<int, TurretBase> turretLocations = GameManager.Instance.MapManager.GetTurretBases();
            LockTheNearestTurret(turretLocations);
        };
        moveState.onFrame = delegate
        {
            if (turretTarget != null)
            {
                if (CalculateDistance(transform.position, turretTarget.position) < attackRange)
                {
                    enemyAgent.isStopped = true;
                    enemyStateMachine.TransitionTo(attackState);
                }
            }
            if (!enemyAgent.pathPending && enemyAgent.remainingDistance < remainDistance)
            {
                if (moveLocations.Count == locationIndex + 1)
                {
                    enemyStateMachine.TransitionTo(deadState);
                    OnDead();
                    GameManager.Instance.OnEnemyEscape();
                    return;
                }
                locationIndex++;
                enemyStateMachine.TransitionTo(moveState);
            }
        };
        attackState = enemyStateMachine.CreateState("attack");
        attackState.onEnter = delegate
        {
            attackTimer = Time.time + 2;
            rotationTimer = Time.time + 2;
        };
        attackState.onFrame = delegate
        {
            CheckTarget();
            if (rotationTimer > Time.time)
            {
                RotateEnemy();
            }
            if (isTargetBeingDestroy)
            {
                enemyStateMachine.TransitionTo(moveState);
            }
            else
            {
                if (attackTimer + 1/attackSpeed < Time.time)
                {
                    Attack();
                    attackTimer = Time.time;
                }
            }
        };
        deadState = enemyStateMachine.CreateState("dead");
    }
    private void RotateEnemy()
    {
        if (turretTarget is null) return;
        Vector3 direction = turretTarget.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime*rotationSpeed);
    }

    private void Attack()
    {
        bulletPool.SetPosition(shootPos);
        bulletPool.Pool.Get();
        Invoke("DamageTurret", attackRange / bulletSpeed);
    }

    private void DamageTurret()
    {
        if (turretTarget is null) return;
        Turret turret = turretTarget?.GetComponent<Turret>();
        if (turret is null) return;
        GameManager.Instance.TurretManager.SendDamage(turret.id,200f);
    }

    private void LockTheNearestTurret(Dictionary<int,TurretBase> turretBases)
    {
        if(turretBases is null || turretBases.Count == 0) return;
        List<TurretBase> turretBasesList = turretBases.Values.ToList();
        float distance = float.MaxValue;
        foreach(var turretBase in turretBasesList)
        {
            if (turretBase.Turret is null) continue;
            float distanceTemp = CalculateDistance(transform.position, turretBase.Turret.transform.position);
            if(distanceTemp < distance) 
            { 
                distance = distanceTemp;
                turretTarget = turretBase.Turret.transform;
                baseTurretID = turretBase.TurretBaseId;
            }
        }
        isTargetBeingDestroy = false;
    }

    private float CalculateDistance(Vector3 point1, Vector3 point2)
    {
        return Vector3.Distance(point2,point1);
    }

    private void CheckTarget()
    {
        if (!GameManager.Instance.MapManager.HasTurret(baseTurretID))
        {
            turretTarget = null;
            enemyAgent.isStopped = false;
            isTargetBeingDestroy = true;
        }
    }

    private void Update()
    {
        enemyStateMachine.Update();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
