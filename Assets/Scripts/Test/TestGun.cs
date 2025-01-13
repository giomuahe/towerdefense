using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestGun : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet1;
    public GameObject firePoint;
    public GameObject targetPoint;
    public float bulletSpeed = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            print("Shoot");
            Instantiate(bullet1, firePoint.transform.position, targetPoint.transform.rotation);

            //Thêm tốc độ và hướng
            Bullet bulletScript = bullet1.GetComponent<Bullet>();
            bulletScript.target = targetPoint;
            bulletScript.speed = bulletSpeed;
        }
    }
}
