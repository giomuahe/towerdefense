using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretInfoSelect : MonoBehaviour
{
    public Text TurretName;
    public Text Description;
    public Text Cost;

    public void SetInfo(string turretName, string description, long cost)
    {
        TurretName.text = turretName;
        Description.text = description;
        Cost.text = cost.ToString("N0");
    }
}
