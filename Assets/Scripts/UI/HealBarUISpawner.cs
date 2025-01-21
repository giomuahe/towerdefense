using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealBarUISpawner : MonoBehaviour
{
    private Slider healthBarSlider;
    private void Awake()
    {
        healthBarSlider = GetComponent<Slider>();
    }

    private void HealthBarSpawner(Transform targetHealthBar)
    {
        Slider newHealthBar = Instantiate(healthBarSlider);
    }

}
