using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    private Transform target;
    private Camera cam;

    public void Initialize(Transform followTarget, float maxHealth)
    {
        target = followTarget;
        cam = Camera.main;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
    }

    public void UpdateHealth(float currentHealth)
    {
        healthBarSlider.value = currentHealth;
    }

    private void LateUpdate()
    {
        if (target == null || cam == null) return;

        transform.position = target.position + offset;
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}
