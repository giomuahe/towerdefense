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
            LockTheNearestTurret(turretLocations);
        };
        moveState.onFrame = delegate
        {
            if(CalculateDistance(transform.position, turretTarget.position) < attackRange)
            {
                enemyStateMachine.TransitionTo(attackState);
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
            Invoke("DamageTurret", attackRange/bulletSpeed);
        }
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
