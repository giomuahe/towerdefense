using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooling: MonoBehaviour
{
    public ObjectPool<GameObject> Pool;
    private GameObject prefab;
    private Transform parentTranform;

    public void CreateNewObjectPool(GameObject prefab)
    {
        Pool = new ObjectPool<GameObject>(CreatePool, OnGetFromPool, OnReleaseFromPool, OnDestroyObject, true, 1000, 10000);
        this.prefab = prefab;
    }

    private void OnDestroyObject(GameObject instance)
    {
        Destroy(instance.gameObject);
    }

    private void OnReleaseFromPool(GameObject instance)
    {
        instance.gameObject.SetActive(false);
    }

    private void OnGetFromPool(GameObject instance)
    {
        instance.transform.position = parentTranform.position;
        instance.transform.right = parentTranform.right;
        // Set active for object
        instance.gameObject.SetActive(true);
    }

    private GameObject CreatePool()
    {
        GameObject instanceObject = Instantiate(prefab, transform);
        instanceObject.GetComponent<ReturnToPool>().Pool = Pool;
        return instanceObject;
    }

    public void SetPosition(Transform parentTranform)
    {
        this.parentTranform = parentTranform;
    }
}






