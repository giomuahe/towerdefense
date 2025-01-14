using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTurretButton : MonoBehaviour
{
   public int ID;
   public TurretType Type;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void SetParam (int idTurret, TurretType turretType){
        this.ID=idTurret;
        this.Type=turretType;
    }
}
