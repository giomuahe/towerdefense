using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireTurret : Turret
{ private const string LINK_TURRET_BULLET_PREFAB = "Turrets/TurretBullet/";
    public float burnDuration = 3f;
    [SerializeField]
    private GameObject FireMuzzle;
    public ParticleSystem flameEffect;
    
    private Dictionary<int, EnemyBase>enemiesInRange = new Dictionary<int, EnemyBase>();
    public override void Initialize()
    {
        base.Initialize();

        bulletPrefab = Resources.Load<GameObject>(LINK_TURRET_BULLET_PREFAB + turretConfig.bulletPrefabPath);
    }
    public override void Fire()
    {
        base.Fire();
        if (CanDamage())
        {
            ApplyDamage();
        }


    }
    public override void UpdateAtkState()
    {
        base.UpdateAtkState();
        CheckEnemiesInCone();
        CheckEnemiesOutCone();
        FireMuzzle.SetActive(enemiesInRange.Count > 0);

        Debug.Log(enemiesInRange.Count);

    }
    private void CheckEnemiesInCone()
    {
        Collider[] colliders = Physics.OverlapSphere(firePos.position, AtkRange); // Tìm tất cả object trong phạm vi hình cầu
        foreach (Collider col in colliders)
        {
            EnemyBase enemy = col.GetComponent<EnemyBase>();
            if (enemy != null && IsInCone(enemy.transform))
            {
                if (!enemiesInRange.ContainsKey(enemy.GetEnemyInGameID()))
                {
                    enemiesInRange[enemy.GetEnemyInGameID()]=enemy;
                }
            }
        }

    }
    private void CheckEnemiesOutCone()
    {
        foreach (var enemy in enemiesInRange.ToList()){
            if(enemy.Value==null){
                enemiesInRange.Remove(enemy.Key);
            }else{
              if(!IsInCone(enemy.Value.transform)){
                enemiesInRange.Remove(enemy.Key);
            }
            }
        }
     
    }
    bool CanDamage()
    {
        return enemiesInRange.Count > 0;
    }

    private bool IsInCone(Transform target)
    {
        Vector3 directionToTarget = (target.position - firePos.position).normalized; // Hướng đến kẻ địch
        float angle = Vector3.Angle(firePos.forward, directionToTarget); // Tính góc giữa hướng súng và hướng đến kẻ địch
        return angle <= AtkAngle;
    }

    private void ApplyDamage()
    {
        foreach (var enemy in enemiesInRange.ToList())
        {
            if(enemy.Value==null){
                return;
            }
           
            if (enemy.Value.transform.Find(turretConfig.bulletPrefabPath) == null)
            {
                GameObject fire = Instantiate(bulletPrefab, enemy.Value.transform.position, Quaternion.identity, enemy.Value.transform);
                fire.name = turretConfig.bulletPrefabPath;
                BurnBase burn = fire.GetComponent<BurnBase>();
                burn.SetDamage(AtkDamage);
                burn.SetDamageRate(AtkSpeed);
                burn.SetTimeExist(burnDuration);
            }
            else
            {
                BurnBase burn = enemy.Value.transform.Find(turretConfig.bulletPrefabPath).GetComponent<BurnBase>();
                burn.SetTimeExist(burnDuration);
            }
            
        }
    }


}


