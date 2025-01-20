using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tes : MonoBehaviour
{
    [SerializeField]
    private Transform a;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(a.position);   
    }


}
