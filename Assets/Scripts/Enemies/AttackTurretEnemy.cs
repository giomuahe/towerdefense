using Managers;
using MapConfigs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AttackTurretEnemy : EnemyBase
{
    protected float enemyTurretDamage;
    protected float attackRange;
    protected float attackSpeed;
    protected float bulletSpeed;
    protected int baseTurretID;
    private GameObject turretTarget;
    protected StateMachine enemyStateMachine;
    private StateMachine.State moveState;
    protected StateMachine.State attackState;
    private StateMachine.State deadState;
    protected bool hasTurretOnRange;
    private bool isTargetBeingDestroy;

    private int bulletID;
    private ObjectPooling bulletPool;

    [SerializeField]
    private Transform shootPos;

    public override void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations, int enemyInGameID)
    {
        attackRange = enemyConfig.AttackRange;
        attackSpeed = enemyConfig.AttackSpeed;
        enemyTurretDamage = enemyConfig.EnemyTurretDamage;
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
                // damge nexus
                enemyStateMachine.TransitionTo(deadState);
                OnDead();
                return;
            }
            Dictionary<int, TurretBase> turretLocations = GameManager.Instance.MapManager.GetTurretBases();
            DecideMoveLocation(turretLocations);
        };
        moveState.onFrame = delegate
        {
            if(hasTurretOnRange && isTargetBeingDestroy)
            {
                enemyStateMachine.TransitionTo(moveState);
            }
            if (!enemyAgent.pathPending && enemyAgent.remainingDistance < remainDistance)
            {
                if(hasTurretOnRange)
                {
                    enemyStateMachine.TransitionTo(attackState);
                }
                else
                {
                    enemyStateMachine.TransitionTo(moveState);
                }
            }
        };
        attackState = enemyStateMachine.CreateState("attack");
        attackState.onEnter += delegate
        {
            StartCoroutine(Attack());
        };
        attackState.onFrame = delegate
        {
            if (isTargetBeingDestroy)
            {
                enemyStateMachine.TransitionTo(moveState);
            }
        };
        deadState = enemyStateMachine.CreateState("dead");
    }

    IEnumerator Attack()
    {
        while(!isTargetBeingDestroy)
        {
            yield return new WaitForSeconds(1 / attackSpeed);
            bulletPool.SetPosition(shootPos);
            bulletPool.Pool.Get();
            Invoke("DamageTurret", 1);
        }
    }

    private void DamageTurret()
    {
        Turret turret = turretTarget?.GetComponent<Turret>();
        if (turret == null) return;
        TurretManager.Instance.SendDamage(turret.id);
    }

    private void DecideMoveLocation(Dictionary<int,TurretBase> turretBases)
    {
        float distance = float.MaxValue;
        Vector3 tempPos = Vector3.zero;
        if(turretBases == null ||  turretBases.Count == 0)
        {
            Move(moveLocations[++locationIndex]);
            hasTurretOnRange = false;
            return;
        }
        foreach(var turretBase in turretBases)
        {
            Vector3 turretPos = turretBase.Value.Position;
            float distance1 = CalculateDistance(transform.position, turretPos);
            if(distance1 < distance)
            {
                distance = distance1;
                baseTurretID = turretBase.Key;
                tempPos = turretPos;
                turretTarget = turretBase.Value.Turret;
            }
        }
        float distance2 = CalculateDistance(transform.position, moveLocations[locationIndex + 1]);
        if(distance2 < distance)
        {
            Move(moveLocations[++locationIndex]);
            hasTurretOnRange = false;
            isTargetBeingDestroy = false;
        }
        else
        {
            Move(tempPos);
            hasTurretOnRange = true;
            enemyAgent.stoppingDistance = attackRange;
        }
    }

    private float CalculateDistance(Vector3 point1, Vector3 point2)
    {
        return Mathf.Sqrt((point1 - point2).magnitude);
    }

    private void CheckTarget()
    {
        if (!GameManager.Instance.MapManager.HasTurret(baseTurretID))
        {
            isTargetBeingDestroy = true;
        }
    }

    private void Update()
    {
        enemyStateMachine.Update();
    }
}
