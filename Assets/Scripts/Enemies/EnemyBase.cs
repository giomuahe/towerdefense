using MapConfigs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected float currentHealth;
    protected float enemyHealth;
    protected float enemySpeed;
    protected float enemyNexusDamage;
    protected EnemyType enemyType;
    private int goldDropAmount;

    protected List<Vector3> moveLocations;
    protected int locationIndex = 0;
    [SerializeField]
    protected float remainDistance;
    protected NavMeshAgent enemyAgent;

    protected int enemyInGameID;
    
    protected virtual void Move(Vector3 destination)
    {
        enemyAgent.destination = destination;
    }

    public virtual void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations, int enemyInGameID)
    {
        this.enemyInGameID = enemyInGameID;
        enemyHealth = enemyConfig.EnemyHealth;
        enemySpeed = enemyConfig.EnemySpeed;
        enemyNexusDamage = enemyConfig.EnemyNexusDamage;
        enemyType = enemyConfig.EnemyType;
        goldDropAmount = enemyConfig.GoldDropAmount;
        this.moveLocations = moveLocations;
        enemyAgent = GetComponent<NavMeshAgent>();
        currentHealth = enemyHealth;
    }

    public int GetEnemyInGameID()
    {
        return enemyInGameID;
    }

    public void OnHit(float damage, out bool isDie)
    {
        isDie = false;
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            OnDead();
            GameManager.Instance.OnEnemyDie(goldDropAmount);
            isDie = true;
        }
    }

    protected void OnDead()
    {
        // gui Enemy Type va destroy object

        Destroy(gameObject);
    }

    public int EnemyID(){
        return enemyInGameID;
    }
}
