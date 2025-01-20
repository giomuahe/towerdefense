using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.VirtualTexturing;

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
