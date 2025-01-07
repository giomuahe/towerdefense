using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.VirtualTexturing;

public class ReturnToPool : MonoBehaviour
{
    void OnParticleSystemStopped()
    {
        // Return to the pool
        ObjectPooling.Instance.Pool.Release(gameObject);
    }
}
