using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TargetType
{
    Barrier,                //�溮
    Crystal,                //ũ����Ż(�ı��� �й�)
}

public class EnemyAI : MonoBehaviour
{
    private Enemy enemy;

    public Transform crystal;                //ũ����Ż
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
