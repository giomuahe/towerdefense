using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuretManager : MonoBehaviour
{
    public static TuretManager Instance; 
    List<Turret> TurretsOnMap=new List<Turret>();
    void Awake(){
        Instance=this;
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    void UpGradeTurret(Transform oldTurret, TurretType newTurret){

    }
   public void ShowUIUpgrade(List<TurretType> listturetNextLevel){
            
    }
}
