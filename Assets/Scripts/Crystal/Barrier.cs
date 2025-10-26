using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Barrier : MonoBehaviour, IDamageable
{
    public static event Action<Barrier> OnBarrierDestoryed;

    public float maxHealth = 50f;
    private float currentHealth;
    private NavMeshSurface surface;

    private void Awake()
    {
        currentHealth = maxHealth;

        surface = FindAnyObjectByType<NavMeshSurface>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
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
