using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    private Dictionary<int, ObjectPooling> bulletPools;

    public void CreateAllPool()
    {
        BulletConfig bulletConfig = Resources.Load<BulletConfig>("Bullet");
        foreach(var bullet in bulletConfig.Bullets)
        {
            ObjectPooling objectPooling = new ObjectPooling();
            objectPooling.CreateNewObjectPool(bullet.BulletPrefab);
            bulletPools.Add(bullet.BulletID, objectPooling);
        }
        Resources.UnloadAsset(bulletConfig);
    }

    public ObjectPooling GetPoolThroughID(int bulletID)
    {
        return bulletPools[bulletID];
    }
}
