using Managers;
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
    protected StateMachine enemyStateMachine;
    private StateMachine.State moveState;
    protected StateMachine.State attackState;
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
        bulletPool = GameManager.Instance.PoolManager.GetPoolThroughID(bulletID);
        enemyStateMachine = new StateMachine();
        CreateState();
    }

    protected virtual void CreateState()
    {
        moveState = enemyStateMachine.CreateState("move");
        moveState.onEnter = delegate
        {
            List<Vector3> turretLocations = new List<Vector3>();
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
        attackState.onEnter += delegate
        {
            StartCoroutine(Attack());
        };
        attackState.onFrame = delegate
        {

        };
    }

    IEnumerator Attack()
    {
        while(!isTargetBeingDestroy)
        {
            //yield return new WaitForSeconds(1 / attackSpeed);
            yield return new WaitForSeconds(1);
            bulletPool.SetPosition(shootPos);
            bulletPool.Pool.Get();
            Invoke("DamageTurret", 1);
        }
    }

    //private void Start()
    //{
    //    bulletID = 100;
    //    bulletPool = GameManager.Instance.PoolManager.GetPoolThroughID(bulletID);
    //    StartCoroutine(Attack());
    //}

    private void DamageTurret()
    {
        // goi den tru gay dmg
    }

    private void DecideMoveLocation(List<Vector3> turretPositions)
    {
        float distance = float.MaxValue;
        Vector3 pos = Vector3.zero;
        if(turretPositions == null ||  turretPositions.Count == 0)
        {
            Move(moveLocations[++locationIndex]);
            hasTurretOnRange = false;
            return;
        }
        foreach(var position in turretPositions)
        {
            float distance1 = CalculateDistance(transform.position, position);
            if(distance1 < distance)
            {
                distance = distance1;
                // set base turretID
                pos = position;
            }
        }
        float distance2 = CalculateDistance(transform.position, moveLocations[locationIndex + 1]);
        if(distance2 < distance)
        {
            Move(moveLocations[++locationIndex]);
            hasTurretOnRange = false;
        }
        else
        {
            Move(pos);
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

    }

    private void Update()
    {
        
    }
}
