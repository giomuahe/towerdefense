using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBurn : BurnBase
{
    public override void Damage()
    {
        base.Damage();
         bool isEnemyDie;
        GameManager.Instance.EnemyManager.SendDamage(EnemyDemo.EnemyID(),  Mathf.RoundToInt(damage), out isEnemyDie);
    }
}
