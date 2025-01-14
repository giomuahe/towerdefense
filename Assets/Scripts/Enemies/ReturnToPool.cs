using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.VirtualTexturing;

public class ReturnToPool : MonoBehaviour
{
    public ObjectPool<GameObject> Pool;
    void OnParticleSystemStopped()
    {
        Pool.Release(gameObject);   
    }
}
