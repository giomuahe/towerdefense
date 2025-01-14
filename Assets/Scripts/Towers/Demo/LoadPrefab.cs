

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoadPrefab
{

    private const string LINK_UI_PREFAB = "UItest/Prefab/";
    private const string LINK_TURRET_PREFAB = "Turrets/Prefab/";

    public Dictionary<TurretType, string> TurretPrefabDic;

    public static Dictionary<TurretEnum, string> LoadEnumToPrefabDic<TurretEnum>() where TurretEnum : Enum
    {
        Dictionary<TurretEnum, string> tDictionary = new Dictionary<TurretEnum, string>();
        foreach (TurretEnum value in Enum.GetValues(typeof(TurretEnum)))
        {
            tDictionary[value] = value.ToString();
        }
        return tDictionary;
    }
    public GameObject LoadTurret(TurretType turretType)
    {
        TurretPrefabDic = LoadEnumToPrefabDic<TurretType>();
        GameObject turret = null;
        turret = Resources.Load<GameObject>(LINK_TURRET_PREFAB + TurretPrefabDic[turretType]);
        return turret;
    }

    public GameObject LoadSelecTurretButton()
    {
        GameObject button = null;
        button = Resources.Load<GameObject>(LINK_UI_PREFAB + "SelectTurretButton");

        return button;

    }

}
