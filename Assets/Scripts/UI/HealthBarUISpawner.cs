using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUISpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarSliderGO;
    private Slider healthBarSlider;
    private void Awake()
    {
        healthBarSlider = healthBarSliderGO.GetComponent<Slider>();
    }

    public void HealthBarSpawner(Transform target)
    {
        Slider newHealthBar = Instantiate(healthBarSlider);
        newHealthBar.gameObject.transform.SetParent(transform);
        HealthBarController healthBarController = newHealthBar.GetComponent<HealthBarController>();
        if (healthBarController is null )
        {
            Debug.Log("Add HealthBarController as component to HealthBarUI");
            Destroy(healthBarSlider.gameObject);
        }
        else
        {
            healthBarController.SetUp(target);
        }
    }

}
