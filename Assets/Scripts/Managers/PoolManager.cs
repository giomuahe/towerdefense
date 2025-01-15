using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    private Dictionary<int, ObjectPooling> bulletPools;
    [SerializeField] private ObjectPooling objectPooling;

    public void CreateAllPool()
    {
        bulletPools = new Dictionary<int, ObjectPooling>();
        BulletConfig bulletConfig = Resources.Load<BulletConfig>("Bullet/BulletList");
        foreach(var bullet in bulletConfig.Bullets)
        {
            ObjectPooling objectPooling = Instantiate(this.objectPooling, transform);
            objectPooling.CreateNewObjectPool(bullet.BulletPrefab);
            bulletPools.Add(bullet.BulletID, objectPooling);
        }
        Resources.UnloadAsset(bulletConfig);
    }

    public ObjectPooling GetPoolThroughID(int bulletID)
    {
        if (!bulletPools.ContainsKey(bulletID))
        {
            Debug.LogError("Add bulletId into object");
        }
        return bulletPools[bulletID];
    }
}
