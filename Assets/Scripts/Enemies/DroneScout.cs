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
                // Gây damage lên nhà chính và destroy object
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
