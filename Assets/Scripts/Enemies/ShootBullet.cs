using UnityEngine;
using UnityEngine.Pool;

public class ShootBullet : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem bullet;
    private ObjectPool<ParticleSystem> pool;
    [SerializeField]
    private Transform shootPos;

    void CreatePool()
    {
        pool = ObjectPooling.Instance.CreateNewObjectPool(10, 100, bullet, shootPos);
    }
    // Start is called before the first frame update
    void Start()
    {
        CreatePool();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            pool.Get();
        }
    }
}
