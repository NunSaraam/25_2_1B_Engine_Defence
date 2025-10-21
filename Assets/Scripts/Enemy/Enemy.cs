using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO data;
    private NavMeshAgent agent;
    private float currentHealth;

    public float attackDamage => data.damage;
    public float MoveSpeed => data.moveSpeed;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        InitiallizeStats();
    }

    void InitiallizeStats()
    {
        currentHealth = data.enemyHealth;
        agent.speed = data.moveSpeed;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
