using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTurretEnemy : EnemyBase
{
    protected float enemyTurretDamage;
    protected float attackRange;
    protected float attackSpeed;
    protected float bulletSpeed;
    private StateMachine enemyStateMachine;
    private StateMachine.State moveState;
    private StateMachine.State attackState;
    private StateMachine.State detectState;
    private bool hasTurretOnRange;

    [SerializeField]
    private Transform shootPos;

    public override void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations, int enemyInGameID)
    {
        attackRange = enemyConfig.AttackRange;
        attackSpeed = enemyConfig.AttackSpeed;
        enemyTurretDamage = enemyConfig.EnemyTurretDamage;
        base.SetUp(enemyConfig, moveLocations, enemyInGameID);
        enemyStateMachine = new StateMachine();
        CreateState();
    }

    void CreateState()
    {
        moveState = enemyStateMachine.CreateState("move");
        moveState.onEnter = delegate
        {
            if (hasTurretOnRange)
            {
                // Move to turret
            }
            else
            {
                Move(moveLocations[locationIndex]);
            }
        };
        moveState.onFrame = delegate
        {
            if(!enemyAgent.pathPending && enemyAgent.remainingDistance < remainDistance)
            {
                
            }
            // if isAttaking and turret explode, move to next location
        };
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackRange / bulletSpeed);
        
    }

    

    private void MoveController()
    {
        if (moveLocations.Count == 0)
        {
            return;
        }
        //Neu dang attack ko di chuyen, khi tru ma con nay dang attack bi pha moi di chuyen tiep
        if (!enemyAgent.pathPending && enemyAgent.remainingDistance < remainDistance)
        {
            if (moveLocations.Count == locationIndex + 1)
            {
                // Gây damage lên nhà chính và destroy object
                return;
            }
            List<Vector3> turretLocation = EnemyManager.Instance.moveLocations;
            if (turretLocation == null)
            {
                MoveAlongAvailablePath();
                Debug.Log("Enemy move locations is missing");
                return;
            }
            if(turretLocation.Count == 0)
            {
                MoveAlongAvailablePath();
                return;
            }
            float distance1 = CalculateDistance(moveLocations[locationIndex], moveLocations[locationIndex + 1]);
            Vector3 destination = Vector3.zero;
            float distance2 = Mathf.Infinity;
            foreach(var location in turretLocation)
            {
                float distance = CalculateDistance(location, moveLocations[locationIndex]);
                if(distance < distance2)
                {
                    distance2 = distance;
                    destination = location;
                }
            }
            if(distance1 < distance2 )
            {
                MoveAlongAvailablePath();
            }
            else
            {
                Move(destination);
            } 
        }
    }

    private void MoveAlongAvailablePath()
    {
        locationIndex += 1;
        Move(moveLocations[locationIndex]);
    }

    private float CalculateDistance(Vector3 point1, Vector3 point2)
    {
        return Mathf.Sqrt((point1 - point2).magnitude);
    }

    private void Update()
    {
        
    }
}
