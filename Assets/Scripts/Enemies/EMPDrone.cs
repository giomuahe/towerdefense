using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPDrone : AttackTurretEnemy
{
    private float skillTimer;
    private float skillCooldown;
    private void DisableTurret()
    {
        if (hasTurretOnRange)
        {
            if(Time.time < skillTimer + skillCooldown)
            {
                return;
            }
            // goi tru ko cho tan cong
            skillTimer = Time.time;
        }
    }
}
