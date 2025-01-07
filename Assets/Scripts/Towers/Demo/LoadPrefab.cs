

using UnityEngine;

public class LoadPrefab 
{
    private const string LINK_UI_PREFAB="UItest/Prefab/";
    public GameObject LoadButtonUpgradeTurret(TurretType turretType){
        GameObject button=null;
        switch(turretType){
            case TurretType.Gatling_Level_1:
            button= Resources.Load<GameObject>(LINK_UI_PREFAB+"ButtonGatling1");
            break;
            case TurretType.Sniper_Level_1:
            button= Resources.Load<GameObject>(LINK_UI_PREFAB+"ButtonSnipper1");
            break;
        }
        return button;
        
    }
}
