using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDemo : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHp;
    public float curHp;
    public Transform axis;
    public bool daming = false;
    void Start()
    {
        maxHp = 1000;
        curHp = maxHp;
        daming = false;
    }

    // Update is called once per frame
    void Update()
    {

        transform.RotateAround(axis.position, Vector3.up, 20f * Time.deltaTime);

        if (daming == true)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
    }
    public void TakeDamage(float damage)
    {
        curHp = curHp - damage;
        daming = true;
        if (curHp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
