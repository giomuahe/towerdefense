using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StealthInfiltrator: AttackTurretEnemy
{
    void ChangeLayer()
    {
        gameObject.layer = 6;
    }

    protected override void CreateState()
    {
        base.CreateState();
        attackState.onEnter += ChangeLayer;
    }
}
