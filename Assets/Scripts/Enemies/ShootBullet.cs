using UnityEngine;
using UnityEngine.Pool;

public class ShootBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    private ObjectPool<GameObject> pool;
    [SerializeField]
    private Transform shootPos;

    // Update is called once per frame
    private void Start()
    {
        Invoke("Shoot", 5);
    }

    void Shoot()
    {
        ObjectPooling.Instance.SetPositionAndPrefab(shootPos, bullet);
        ObjectPooling.Instance.Pool.Get();
    }
}
