public class DroneScout : EnemyBase, IHittable 
{
    public void OnHit(float damage)
    {
        
    }

    private void MoveController()
    {
        if(moveLocations.Count == 0)
        {
            return;
        }
        if(!enemyAgent.pathPending && enemyAgent.remainingDistance < remainDistance)
        {
            if(moveLocations.Count == locationIndex + 1)
            {
                // G�y damage l�n nh� ch�nh v� destroy object
                return;
            }
            locationIndex += 1;
            Move(moveLocations[locationIndex]);  
        }
    }
}
