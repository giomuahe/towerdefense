using Managers;
using MapConfigs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private float timer;

    public override void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations, int enemyInGameID)
    {
        attackRange = enemyConfig.AttackRange;
        attackSpeed = enemyConfig.AttackSpeed;
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
            if(moveLocations.Count == locationIndex + 1)
            {
                enemyStateMachine.TransitionTo(deadState);
                OnDead();
                GameManager.Instance.OnEnemyEscape();
                return;
            }
            Move(moveLocations[locationIndex]);
            Dictionary<int, TurretBase> turretLocations = GameManager.Instance.MapManager.GetTurretBases();
            LockTheNearestTurret(turretLocations);
        };
        moveState.onFrame = delegate
        {
            if(turretTarget != null)
            {
                if (CalculateDistance(transform.position, turretTarget.position) < attackRange + 6)
                {
                    enemyAgent.speed = 2;
                    Debug.Log("a");
                }
                if (CalculateDistance(transform.position, turretTarget.position) < attackRange)
                {
                    enemyAgent.isStopped = true;
                    enemyStateMachine.TransitionTo(attackState);
                }
            }
            if (!enemyAgent.pathPending && enemyAgent.remainingDistance < remainDistance)
            {
                locationIndex++;
                enemyStateMachine.TransitionTo(moveState);
            }
        };
        attackState = enemyStateMachine.CreateState("attack");
        attackState.onEnter = delegate
        {
            CheckTarget();
            timer = Time.time + 1;
            transform.LookAt(turretTarget.position);
        };
            attackState.onFrame = delegate
        {
            if (isTargetBeingDestroy)
            {
                enemyStateMachine.TransitionTo(moveState);
            }
            else
            {
                if (timer + 1/attackSpeed < Time.time)
                {
                    Attack();
                    timer = Time.time;
                }
            }
        };
        deadState = enemyStateMachine.CreateState("dead");
    }

    private void Attack()
    {
        bulletPool.SetPosition(shootPos);
        Debug.Log(shootPos.position);
        bulletPool.Pool.Get();
        Invoke("DamageTurret", attackRange / bulletSpeed);
    }

    private void DamageTurret()
    {
        Turret turret = turretTarget?.GetComponent<Turret>();
        if (turret == null) return;
        GameManager.Instance.TurretManager.SendDamage(turret.id,200f);
    }

    private void LockTheNearestTurret(Dictionary<int,TurretBase> turretBases)
    {
        if(turretBases == null || turretBases.Count == 0) return;
        List<TurretBase> turretBasesList = turretBases.Values.ToList();
        float distance = float.MaxValue;
        foreach(var turretBase in turretBasesList)
        {
            if (turretBase.Turret == null) continue;
            float distanceTemp = CalculateDistance(transform.position, turretBase.Turret.transform.position);
            if(distanceTemp < distance) 
            { 
                distance = distanceTemp;
                turretTarget = turretBase.Turret.transform;
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
            //isTargetBeingDestroy = true;
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
