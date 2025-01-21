using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class ReturnToPool : MonoBehaviour
{
    public ObjectPool<GameObject> Pool;
    //void OnParticleSystemStopped()
    //{
    //    ReleaseBullet();   
    //}

    private void ReleaseBullet()
    {
        Pool.Release(gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        Pool.Release(gameObject);
    }
}
