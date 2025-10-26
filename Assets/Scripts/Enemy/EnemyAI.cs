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
    [SerializeField]private Transform currentTargetPoint;

    private bool isAttacking = false;

    private void OnEnable()
    {
        Barrier.OnBarrierDestoryed += HandleBarrierDestroyed;
    }

    private void OnDisable()
    {
        Barrier.OnBarrierDestoryed -= HandleBarrierDestroyed;
    }

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

        float distance = Vector3.Distance(transform.position, currentTarget.position);

        if (distance <= enemy.data.attackRange)
        {
            agent.isStopped = true;

            if (!isAttacking)
            {
                StartCoroutine(HandleAttack());
            }
        }
        else
        { 
            agent.isStopped = false;
            if (agent != null && agent.isOnNavMesh && currentTargetPoint != null)
            {
                agent.SetDestination(currentTargetPoint.position);
            }
        }



    }

    void UpdateTarget()
    {
        Transform newTarget = crystal;

        if (Physics.Raycast(transform.position, (crystal.position - transform.position).normalized,
            out RaycastHit hit, Mathf.Infinity, barrierLayer | LayerMask.GetMask("Default")))
        {
            if (((1 << hit.collider.gameObject.layer) & barrierLayer) != 0)
            {
                newTarget = hit.transform;
            }
        }

        currentTarget = newTarget;

        AttackPositionManager posManager = currentTarget.GetComponent<AttackPositionManager>();
        if (posManager != null)
        {
            currentTargetPoint = posManager.GetNearestAvailablePoint(transform.position);

            if (currentTargetPoint == null)
            {
                currentTargetPoint = CreateRandomPointAroundTarget(currentTarget, 3f);
            }
        }
        else
        {
            currentTargetPoint = CreateRandomPointAroundTarget(currentTarget, 3f);
        }
    }

    IEnumerator HandleAttack()
    {
        isAttacking = true;

        while (currentTarget != null &&
                Vector3.Distance(transform.position, currentTarget.position) <= enemy.data.attackRange)
        {
            IDamageable target = currentTarget.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(enemy.data.damage);
            }

            yield return new WaitForSeconds(enemy.data.attackCoolDown);
        }

        currentTargetPoint = null;
        isAttacking = false;
    }

    Transform CreateRandomPointAroundTarget(Transform target, float radius)
    {
        Vector3 randomDir = Random.insideUnitSphere * radius;
        randomDir.y = 0f;
        randomDir += target.position;

        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            GameObject temp = new GameObject("TempAttackPoint");
            temp.transform.position = hit.position;
            temp.transform.parent = target;
            return temp.transform;
        }

        return target;
    }

    private void HandleBarrierDestroyed(Barrier destroyerBarrier)
    {
        if (currentTarget == null || currentTarget == destroyerBarrier.transform)
        {
            UpdateTarget();
        }
    }
}
