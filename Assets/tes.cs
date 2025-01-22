using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tes : MonoBehaviour
{
    [SerializeField]
    private GameObject a;
    [SerializeField]
    private Transform b;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 1, 2);
    }

    private void Shoot()
    {
        Instantiate(a, b);
    }

}
