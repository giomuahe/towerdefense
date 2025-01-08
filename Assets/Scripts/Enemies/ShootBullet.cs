using UnityEngine;
using UnityEngine.Pool;

public class ShootBullet : MonoBehaviour
{
    [SerializeField]
    private Transform shootPos;

    // Update is called once per frame
    private void Start()
    {
        InvokeRepeating("Shoot", 1, 2);
    }

    void Shoot()
    {
        
    }
}
