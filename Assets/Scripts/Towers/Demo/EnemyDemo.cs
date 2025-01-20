using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDemo : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHp;
    public float curHp;
    public Transform axis;
    void Start()
    {
        maxHp=1000;
        curHp=maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
         transform.RotateAround(axis.position, Vector3.up, 20f * Time.deltaTime);
        
    }
    public void TakeDamage(float damage){
        curHp = curHp-damage;
        if(curHp<=0){
            Die();
        }
    }
    public void Die(){
        Destroy(gameObject);
    }
}
