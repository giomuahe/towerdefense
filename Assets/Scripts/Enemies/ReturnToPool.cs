using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.VirtualTexturing;

public class ReturnToPool : MonoBehaviour
{
    public ObjectPool<ParticleSystem> Pool;
    private ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void OnParticleSystemStopped()
    {
        // Return to the pool
        Pool.Release(particleSystem);
    }
}
