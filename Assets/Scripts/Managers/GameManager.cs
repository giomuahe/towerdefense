using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void NotifyOnTurretHit(int ID)
    {
        // lấy đc trụ từ ID xong gọi OnHit của trụ
    }
}
