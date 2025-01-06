using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected float enemyHealth;
    protected float enemySpeed;
    
    protected float enemyNexusDamage;
    
    protected int enemyID;

    protected List<Vector3> moveLocations;
    protected int locationIndex = 0;
    [SerializeField]
    protected float remainDistance;
    protected NavMeshAgent enemyAgent;
    
    protected virtual void Move(Vector3 destination)
    {
        enemyAgent.destination = destination;
    }

    protected virtual void OnDead()
    {
        Destroy(gameObject);
        // DropGold(enemyID);
    }

    public virtual void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations)
    {
        enemyHealth = enemyConfig.EnemyHealth;
        enemySpeed = enemyConfig.EnemySpeed;
        enemyNexusDamage = enemyConfig.EnemyNexusDamage;
        enemyID = enemyConfig.enemyID;
        this.moveLocations = moveLocations;
        enemyAgent = GetComponent<NavMeshAgent>();
        Move(this.moveLocations[locationIndex]);
    }
}
