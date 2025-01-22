using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnBase : MonoBehaviour
{
    protected float timeExist;
    protected float damage;
    protected float damageRate;
    protected float damageCooldown;
    protected float timeRemain;
    protected EnemyBase EnemyDemo;

    void Start()
    {
        timeRemain = timeExist;
        damageCooldown = 0;
        EnemyDemo = GetComponentInParent<EnemyBase>();

    }


    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetTimeExist(float time)
    {
        this.timeExist = time;
    }
    public void SetDamageRate(float damagerate)
    {
        this.damageRate = damagerate;
    }

    void Update()
    {
        if (CanDamage())
        {
            Damage();
        }
        if (CanDestroy())
        {
            Destroy();
        }

    }
    bool CanDamage()
    {
        if (damageCooldown <= 0)
        {
            return true;
        }
        else
        {
            damageCooldown -= Time.deltaTime;
            return false;
        }
    }
    bool CanDestroy()
    {
        if (timeRemain <= 0)
        {
            return true;
        }
        else
        {
            timeRemain -= Time.deltaTime;
            return false;
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }

    public virtual void Damage()
    {
        damageCooldown = 1 / damageRate;
       

    }
}
