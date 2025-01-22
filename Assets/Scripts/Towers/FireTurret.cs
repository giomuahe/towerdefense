using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireTurret : Turret
{
    private const string LINK_TURRET_BULLET_PREFAB = "Turrets/TurretBullet/";

    private Dictionary<EnemyBase, GameObject> burningEnemies = new Dictionary<EnemyBase, GameObject>();
    public float burnDuration = 3f;
    public float fireRate = 1f;
    public float fireRateTime;
    [SerializeField]
    private GameObject FireMuzzle;
    public ParticleSystem flameEffect;
    private List<EnemyBase> enemiesInRange = new List<EnemyBase>();  // Danh sách enemy trong vùng
    private Dictionary<EnemyBase, Coroutine> burnCoroutines = new Dictionary<EnemyBase, Coroutine>();


    public override void Initialize()
    {
        base.Initialize();

        bulletPrefab = Resources.Load<GameObject>(LINK_TURRET_BULLET_PREFAB + turretConfig.bulletPrefabPath);
    }
    public override void Fire()
    {
        base.Fire();
        CheckEnemiesInCone();
        ApplyDamage();

    }
    public override void UpdateAtkState()
    {
        base.UpdateAtkState();
        FireMuzzle.SetActive(isAttacking);
        if(fireRateTime<=0){
            fireRateTime=0;
        }
    }
    private void CheckEnemiesInCone()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, AtkRange); // Tìm tất cả object trong phạm vi hình cầu
        foreach (Collider col in colliders)
        {
            EnemyBase enemy = col.GetComponent<EnemyBase>();
            if (enemy != null && IsInCone(enemy.transform))
            {
                if (!enemiesInRange.Contains(enemy))
                {
                    enemiesInRange.Add(enemy);
                    if (burnCoroutines.ContainsKey(enemy)) // Nếu có coroutine đốt sau khi rời, dừng lại
                    {
                        StopCoroutine(burnCoroutines[enemy]);
                        burnCoroutines.Remove(enemy);
                    }
                }
            }
        }

        // Kiểm tra enemy nào đã rời khỏi vùng
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            EnemyBase enemy = enemiesInRange[i];
            if(enemy = enemiesInRange[i]){
            if (!IsInCone(enemy.transform))
            {
                enemiesInRange.Remove(enemy);
                burnCoroutines[enemy] = StartCoroutine(BurnAfterExit(enemy)); // Gây sát thương thêm 4 giây
            }
            }
        }

        // Hiển thị hiệu ứng lửa nếu có enemy
        if (enemiesInRange.Count > 0)
        {
            if (!flameEffect.isPlaying)
                flameEffect.Play();
        }
        else
        {
            if (flameEffect.isPlaying)
                flameEffect.Stop();
        }
    }

    private bool IsInCone(Transform target)
    {
        Vector3 directionToTarget = (target.position - firePos.position).normalized; // Hướng đến kẻ địch
        float angle = Vector3.Angle(firePos.forward, directionToTarget); // Tính góc giữa hướng súng và hướng đến kẻ địch
        return angle <= AtkAngle;
    }

    private void ApplyDamage()
    {
        foreach (EnemyBase enemy in enemiesInRange)
        {
            
                 bool isEnemyDie;
        GameManager.Instance.EnemyManager.SendDamage(enemy.EnemyID(), Mathf.RoundToInt(AtkDamage), out isEnemyDie);
             
              

            // Gây sát thương liên tục
        }
    }
    
    


    private IEnumerator BurnAfterExit(EnemyBase enemy)
    {
        float elapsed = 0f;
         GameObject fireBurn=Instantiate(bulletPrefab, enemy.transform.position, Quaternion.identity, enemy.transform);
        while (elapsed < burnDuration)
        {
            if (fireRateTime <= 0)
            {
                  bool isEnemyDie;
               GameManager.Instance.EnemyManager.SendDamage(enemy.EnemyID(), Mathf.RoundToInt(AtkDamage), out isEnemyDie);
                fireRateTime = fireRate;
            }
            else
            {
                fireRateTime -= Time.deltaTime;
            } // Gây sát thương trong 4 giây sau khi rời phạm vi
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(fireBurn);
        burnCoroutines.Remove(enemy); // Xóa khỏi danh sách khi hết thời gian đốt
    }
}


