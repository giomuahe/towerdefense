using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTurret : Turret
{
    private const string LINK_TURRET_BULLET_PREFAB = "Turrets/TurretBullet/";
    private GameObject fireObject;
    public override void Initialize()
    {
        base.Initialize();

        bulletPrefab = Resources.Load<GameObject>(LINK_TURRET_BULLET_PREFAB + turretConfig.bulletPrefabPath);
        fireObject = Resources.Load<GameObject>(LINK_TURRET_BULLET_PREFAB + "Burn");

    }
    public override void Fire()
    {
        base.Fire();

        Collider[] enemies = Physics.OverlapSphere(firePos.position, AtkRange);
        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Vector3 directionToEnemy = (enemy.transform.position - firePos.position).normalized;
                float angle = Vector3.Angle(firePos.forward, directionToEnemy);
                if (angle <= 45f / 2)
                {
                    GameObject fireEffect = Instantiate(fireObject, enemy.transform.position, Quaternion.identity);
                    fireEffect.transform.SetParent(target);
                   BurningFire burning= fireEffect.GetComponent<BurningFire>();
                   burning.SetATKSpeed(AtkSpeed);
                   burning.SetDuration(4f);
                   burning.SetDamage(AtkDamage);
                   
                }
            }
        }
    }
}
