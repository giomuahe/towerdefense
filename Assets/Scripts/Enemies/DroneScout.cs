public class DroneScout : EnemyBase, IHittable 
{
    private void MoveController()
    {
        if(moveLocations.Count == 0)
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
