using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    public Image fillImage;
    public Transform target;  // Àû Transform
    public Vector3 offset = new Vector3(0, 2.5f, 0);

    private Camera cam;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        cam = Camera.main;


        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
            canvas.worldCamera = cam;


        if (target != null)
            transform.position = target.position + offset;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }


        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);


        transform.position = target.position + offset;
    }

    public void UpdateHealth(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }
}
