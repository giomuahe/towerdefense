using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretInfoSelect : MonoBehaviour
{
    public Text TurretName;
    public Text Description;

    public void SetInfo(string turretName, string description)
    {
        TurretName.text = turretName;
        Description.text = description;
    }
}
