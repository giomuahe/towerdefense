using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooling : MonoBehaviour
{
    private ObjectPool<GameObject> pool;
    private GameObject prefab;
    private Transform parentTranform;

    // tạo 1 biến ObjectPool<GameObject> để giữ lưu lại giá trị pool còn sử dụng để Release và get.
    public ObjectPool<GameObject> CreateNewObjectPool(int defaultCapacity, int maxCapacity, GameObject prefab, Transform parentTranform)
    {
        this.parentTranform = parentTranform;
        this.prefab = prefab;
        pool = new ObjectPool<GameObject>(CreatePool, OnGetFromPool, OnReleaseFromPool, OnDestroyObject, true, defaultCapacity, maxCapacity);
        return pool;
    }

    private void OnDestroyObject(GameObject instance)
    {
        Destroy(instance);
    }

    private void OnReleaseFromPool(GameObject instance)
    {
        instance.SetActive(false);
    }

    private void OnGetFromPool(GameObject instance)
    {
        // Set object to the default tranform
        instance.transform.position = parentTranform.transform.position;
        instance.transform.right = parentTranform.transform.right;
        // Set active for object
        instance.SetActive(true);

    }

    private GameObject CreatePool()
    {
        GameObject instanceObject = Instantiate(prefab, parentTranform);
        return instanceObject;
    }
}
