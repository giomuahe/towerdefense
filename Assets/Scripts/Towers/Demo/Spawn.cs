using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemy;
    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, 2f);
    }

    
    void Update()
    {
        
        
    }
    void SpawnEnemy(){
         Instantiate(enemy, transform.position, transform.rotation);
    }
}
