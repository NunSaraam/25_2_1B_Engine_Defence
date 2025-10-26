using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public EnemySO data;
    private NavMeshAgent agent;
    public float currentHealth { get; private set; }

    public float attackDamage => data.damage;
    public float MoveSpeed => data.moveSpeed;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = data.enemyHealth;

        var agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.speed = data.moveSpeed;

        //InitiallizeStats();
    }

    //void InitiallizeStats()
    //{

    //}

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
