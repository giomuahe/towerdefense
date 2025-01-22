using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperTurret : Turret
{

 private const string LINK_TURRET_BULLET_PREFAB="Turrets/TurretBullet/";
    public override void Initialize()
    {
        base.Initialize();

bulletPrefab= Resources.Load<GameObject>(LINK_TURRET_BULLET_PREFAB + turretConfig.bulletPrefabPath);

    }
    public override void Fire()
    {
        base.Fire();
        GameObject bulletObj = Instantiate(bulletPrefab, firePos.position, Quaternion.identity);
        TurretBullet turretBullet = bulletObj.GetComponent<TurretBullet>();
        turretBullet.SetTarget(target);
         turretBullet.SetSpeed(BulletSpeed);
         turretBullet.SetDamage(AtkDamage);
        // // GameObject muzzleEffect = Instantiate(turretBullet.GetFlash(), firePos.position, firePos.rotation);
        // Destroy(muzzleEffect, 1);
    }
}
