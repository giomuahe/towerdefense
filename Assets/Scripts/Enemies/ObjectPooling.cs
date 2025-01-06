using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooling: MonoBehaviour
{
    private ObjectPool<ParticleSystem> pool;
    private ParticleSystem prefab;
    private Transform parentTranform;
    public static ObjectPooling Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // tạo 1 biến ObjectPool<GameObject> để giữ lưu lại giá trị pool còn sử dụng để Release và get.
    public ObjectPool<ParticleSystem> CreateNewObjectPool(int defaultCapacity, int maxCapacity, ParticleSystem prefab, Transform parentTranform)
    {
        this.parentTranform = parentTranform;
        this.prefab = prefab;
        pool = new ObjectPool<ParticleSystem>(CreatePool, OnGetFromPool, OnReleaseFromPool, OnDestroyObject, true, defaultCapacity, maxCapacity);
        return pool;
    }

    private void OnDestroyObject(ParticleSystem instance)
    {
        //Destroy(instance.gameObject);
    }

    private void OnReleaseFromPool(ParticleSystem instance)
    {
        instance.gameObject.SetActive(false);
    }

    private void OnGetFromPool(ParticleSystem instance)
    {
        // Set object to the default tranform
        instance.transform.position = parentTranform.transform.position;
        instance.transform.right = parentTranform.transform.right;
        // Set active for object
        instance.gameObject.SetActive(true);

    }

    private ParticleSystem CreatePool()
    {
        ParticleSystem instanceObject = Instantiate(prefab, parentTranform);
        var returnToPool = instanceObject.GetComponent<ReturnToPool>();
        if(returnToPool == null)
        {
            Debug.Log("Add script ReturnToPool to the bullet prefab");
        }
        else
        {
            returnToPool.Pool = pool;
        }
        return instanceObject;
    }
}
