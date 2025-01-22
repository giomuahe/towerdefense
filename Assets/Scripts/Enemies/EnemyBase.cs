using MapConfigs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour, IHealthBar
{
    protected float currentHealth;
    protected float enemyHealth;
    protected float enemySpeed;
    protected float enemyNexusDamage;
    private int goldDropAmount;

    protected List<Vector3> moveLocations;
    protected int locationIndex = 0;
    [SerializeField]
    protected float remainDistance;
    protected NavMeshAgent enemyAgent;

    protected int enemyInGameID;
    private bool isFirstTimeBeingAttack = false;

    public event Action<float> OnHealthChange;

    protected virtual void Move(Vector3 destination)
    {
        enemyAgent.destination = destination;
    }

    public virtual void SetUp(EnemyConfig enemyConfig, List<Vector3> moveLocations, int enemyInGameID)
    {
        this.enemyInGameID = enemyInGameID;
        enemyHealth = enemyConfig.EnemyHealth;
        enemySpeed = enemyConfig.EnemySpeed;
        goldDropAmount = enemyConfig.GoldDropAmount;
        this.moveLocations = moveLocations;
        enemyAgent = GetComponent<NavMeshAgent>();
        enemyAgent.speed = enemySpeed;
        currentHealth = enemyHealth;
    }
    public int GetEnemyInGameID()
    {
        return enemyInGameID;
    }

    public void OnHit(float damage, out bool isDie)
    {
        if (!isFirstTimeBeingAttack)
        {
            GameManager.Instance.UIManager.SpawnHealthBarUI(transform);
            isFirstTimeBeingAttack = true;
        }
        isDie = false;
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            OnDead();
            GameManager.Instance.OnEnemyDie(goldDropAmount);
            GameManager.Instance.EnemyManager.RemoveEnemyFromDic(enemyInGameID);
            isDie = true;
        }
        OnHealthChange.Invoke(currentHealth/enemyHealth);
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
