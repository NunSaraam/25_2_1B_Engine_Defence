using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    public EnemySO data;
    private NavMeshAgent agent;

    public GameObject healthBarPrefab;
    private EnemyHealthBar healthBarInstance;

    public float currentHealth { get; private set; }

    public float attackDamage => data.damage;
    public float MoveSpeed => data.moveSpeed;
    public bool IsDead => currentHealth <= 0;

    public int goldReward = 50;

    private void Awake()
    {
        InitiallizeStats();
    }

    private void Start()
    {
        if (healthBarPrefab != null)
        {
            GameObject hb = Instantiate(healthBarPrefab);
            healthBarInstance = hb.GetComponent<EnemyHealthBar>();
            healthBarInstance.target = transform;
        }
    }

    void InitiallizeStats()
    {
        currentHealth = data.enemyHealth;
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.speed = data.moveSpeed;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (healthBarInstance != null)
            healthBarInstance.UpdateHealth(currentHealth, data.enemyHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (ShopManager.Instance != null)
        {
            ShopManager.Instance.AddGold(goldReward);
        }
        if (healthBarInstance != null)
            Destroy(healthBarInstance.gameObject);
        Destroy(gameObject);
    }
}
