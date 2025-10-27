using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, IDamageable
{
    public float maxHealth = 200f;
    private float currentHealth;
    private HealthBarUI healthBar;

    public GameObject gameoverPanel;

    private Vector3 offset = new Vector3(0, 7f, 0);

    private void Awake()
    {
        currentHealth = maxHealth;
        gameoverPanel.SetActive(false);
    }

    private void Start()
    {
        GameObject barPrefab = Resources.Load<GameObject>("UI/HealthBar");
        if (barPrefab != null)
        {
            GameObject bar = Instantiate(barPrefab, transform.position, Quaternion.identity);
            healthBar = bar.GetComponent<HealthBarUI>();
            healthBar.Initialize(transform, maxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.UpdateHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Time.timeScale = 0f;
            Destroy(gameObject);
            gameoverPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
