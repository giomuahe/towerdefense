using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningFire : MonoBehaviour
{
    private float damage;
    private float duration;
    private float atkSpeed;
    private float existTime;
    private float attackCooldown;
    ParticleSystem particle;

    void Start()
    {
        particle= GetComponent<ParticleSystem>();
        existTime = duration;
        attackCooldown = 0;
    }
    public void SetDamage(float turretDamage)
    {
        this.damage = turretDamage;
    }
    public void SetDuration(float durationtime)
    {
        this.duration = durationtime;
    }
    public void SetATKSpeed(float atkSpeed)
    {
        this.atkSpeed = atkSpeed;
    }
    
    void Update()
    {
        DurationCountDown();
        if (attackCooldown <= 0)
        {
            attackCooldown = 0;
        }
        if (CanAttack())
        {
            Attack();
        }else{
            particle.Stop();
        }
    }
    void DurationCountDown()
    {

        if (existTime > 0)
        {
            existTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);

        }
    }
    bool CanAttack()
    {
        if (attackCooldown <= 0)
        {
            return true;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
            return false;
        }
    }
    void Attack()
    {
        EnemyDemo enemy = GetComponentInParent<EnemyDemo>();
        enemy.TakeDamage(damage);
        particle.Play();
        attackCooldown = 1 / atkSpeed;
    }
}
