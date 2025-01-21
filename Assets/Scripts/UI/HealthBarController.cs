using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Transform target;
    private RectTransform rectTransform;
    [SerializeField]
    private Vector3 offset;
    private Slider healthBarSlider;
    public void SetUp(Transform target)
    {
        this.target = target;
        rectTransform = GetComponent<RectTransform>();
        healthBarSlider = GetComponent<Slider>();
        IHealthBar healthBar = target.GetComponent<IHealthBar>();
        healthBar.OnHealthChange += ChangeHealthBarValue;
    }

    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (target != null)
        {
            // Chuyển đổi vị trí 3D của nhân vật sang vị trí 2D của UI Canvas.
            Vector3 worldPosition = target.position + offset;

            // Cập nhật vị trí của thanh máu.
            rectTransform.position = worldPosition;

            // Xoay thanh máu theo góc xoay của nhân vật.
            rectTransform.rotation = Quaternion.Euler(90, target.eulerAngles.y, 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ChangeHealthBarValue(float healthPercent)
    {
        healthBarSlider.value = healthPercent;
    }
}
