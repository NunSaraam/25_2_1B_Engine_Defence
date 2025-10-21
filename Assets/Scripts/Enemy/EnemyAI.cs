using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TargetType
{
    Barrier,                //¹æº®
    Crystal,                //Å©¸®½ºÅ»(ÆÄ±«½Ã ÆÐ¹è)
}

public class EnemyAI : MonoBehaviour
{
    private Enemy enemy;

    public Transform crystal;                //Å©¸®½ºÅ»
    public LayerMask barrierLayer;
    private NavMeshAgent agent;
    private Transform currentTarget;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        UpdateTarget();
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            UpdateTarget();
            return;
        }

        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(currentTarget.position);
        }

    }

    void UpdateTarget()
    {
        if (Physics.Raycast(transform.position, (crystal.position - transform.position).normalized,
            out RaycastHit hit, Mathf.Infinity, barrierLayer | LayerMask.GetMask("Default")))
        {
            if (((1 << hit.collider.gameObject.layer) & barrierLayer) != 0)
            {
                currentTarget = hit.transform;
                return;
            }
        }

        currentTarget = crystal;
    }
}
