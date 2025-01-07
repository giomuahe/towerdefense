using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;         // Điểm B (đích)
    public float speed = 10f;        // Tốc độ bắn của viên đạn
    public float explosionRadius = 5f; // Bán kính nổ khi chạm đích


    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Debug.Log("DIR = " + direction);
        Debug.Log("SPEED = " + speed);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == target)
        {
            //Trúng
            Debug.Log("No dan");
            //Xử lý viên đạn vào objectpool hoặc destroy nó
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
