using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDemo : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxHp;
    public float curHp;
    void Start()
    {
        maxHp=300;
        curHp=maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position+=new Vector3(0,0,0) * Time.deltaTime;
        
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
