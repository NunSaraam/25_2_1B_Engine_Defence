using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Barrier : MonoBehaviour, IDamageable
{
    public static event Action<Barrier> OnBarrierDestoryed;

    public float maxHealth = 100f;
    private float currentHealth;

    private NavMeshSurface surface;
    private HealthBarUI healthBar;

    private void Awake()
    {
        currentHealth = maxHealth;

        surface = FindAnyObjectByType<NavMeshSurface>();
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
            OnBarrierDestoryed?.Invoke(this);
            DestroyBarrier();
        }
    }

    private void DestroyBarrier()
    {
        Destroy(gameObject);

        if (surface != null)
        {
            surface.BuildNavMesh();
        }
    }
}
