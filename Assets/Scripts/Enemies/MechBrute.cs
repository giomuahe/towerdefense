using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechBrute : AttackTurretEnemy
{
    private float recoveryAmount;
    private float skill1Timer;
    private float skill1Cooldown;

    private void Regenerate()
    {
        if(Time.time < skill1Timer + skill1Cooldown)
        {
            return;
        }
        if(currentHealth < enemyHealth)
        {
            currentHealth += recoveryAmount;
            if(currentHealth > enemyHealth )
            {
                currentHealth = enemyHealth;
            }
            skill1Timer = Time.time;
        }
    }
    
    private void Taunt()
    {
        // tao ra 1 vung dan ko ban qua dc
    }
}
