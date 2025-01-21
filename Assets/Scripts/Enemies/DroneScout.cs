using System.Collections.Generic;
using UnityEngine;

public class DroneScout : EnemyBase, IHittable 
{
    public override void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations, int enemyInGameID)
    {
        base.SetUp(enemyConfig, moveLocations, enemyInGameID);
        Move(moveLocations[locationIndex]);
    }
    private void MoveController()
    {
        if(moveLocations.Count == 0 || moveLocations is null)
        {
            return;
        }
        if(!enemyAgent.pathPending && enemyAgent.remainingDistance < remainDistance)
        {
            if (moveLocations.Count == locationIndex + 1)
            {
                OnDead();
                GameManager.Instance.OnEnemyEscape();
                GameManager.Instance.EnemyManager.RemoveEnemyFromDic(enemyInGameID);
                return;
            }
            Move(moveLocations[++locationIndex]);
        }
    }

    private void Update()
    {
        MoveController();
    }
}
