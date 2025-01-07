using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    public void OnHit(float damage);
}

public interface IAttackable
{
    public void OnAttack();
}

public interface ISkill
{
    public void OnUsingSkill();
}
