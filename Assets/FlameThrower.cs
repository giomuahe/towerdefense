using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> enemiesInRange = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
         Debug.Log("s"+other);
        if (other.CompareTag("Enemy"))
        {
            // if (!enemiesInRange.Contains(other.gameObject))
            // {
            // enemiesInRange.Add(other.gameObject);
            EnemyDemo enemyDemo = other.GetComponent<EnemyDemo>();
           
            // }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Remove(other.gameObject);

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
