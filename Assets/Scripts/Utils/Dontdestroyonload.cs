using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<DontDestroyOnLoad>().Length > 1)
        {
            Debug.LogWarning("Init 2 object type [DontDestroyOnLoad]");
            Destroy(gameObject); 
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
