using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TargetType
{
    Barrier,                //방벽
    Crystal,                //크리스탈(파괴시 패배)
}

public class EnemyAI : MonoBehaviour
{
    private Enemy enemy;

    public Transform crystal;
    public LayerMask barrierLayer;

    private NavMeshAgent agent;
    private Transform currentTarget;
    [SerializeField] private Transform currentTargetPoint;

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

        float distanceToTarget = GetDistanceToTarget();
        float adjustedRange = GetAdjustedRange(currentTarget); //변경된 거리 함수 사용

        if (distanceToTarget <= adjustedRange)
        {
            agent.isStopped = true;

            if (!isAttacking)
                StartCoroutine(HandleAttack(adjustedRange));
        }
        else
        {
            agent.isStopped = false;

            if (agent != null && agent.isOnNavMesh)
            {
                if (currentTargetPoint != null)
                    agent.SetDestination(currentTargetPoint.position);
                else
                    agent.SetDestination(currentTarget.position);
            }
        }
    }

    void UpdateTarget()
    {
        Transform newTarget = crystal;

        Vector3 start = transform.position + Vector3.up * 1.0f;
        Vector3 dirToCrystal = (crystal.position - start).normalized;

        // Barrier 우선 감지
        if (Physics.Raycast(start, dirToCrystal, out RaycastHit hit, Mathf.Infinity, barrierLayer | LayerMask.GetMask("Default")))
        {
            if (((1 << hit.collider.gameObject.layer) & barrierLayer.value) != 0)
            {
                newTarget = hit.transform;
                Debug.DrawLine(start, hit.point, Color.red, 0.3f); // Barrier hit
            }
            else
            {
                Debug.DrawLine(start, hit.point, Color.yellow, 0.3f); // Crystal or other
            }
        }

        currentTarget = newTarget;

        // 공격 포인트 설정
        AttackPositionManager posManager = currentTarget.GetComponent<AttackPositionManager>();
        if (posManager != null)
        {
            currentTargetPoint = posManager.GetNearestAvailablePoint(transform.position);
            if (currentTargetPoint == null)
                currentTargetPoint = CreateRandomPointAroundTarget(currentTarget, 3f);
        }
        else
        {
            currentTargetPoint = CreateRandomPointAroundTarget(currentTarget, 3f);
        }

        if (agent != null && agent.isOnNavMesh)
        {
            if (currentTargetPoint != null)
                agent.SetDestination(currentTargetPoint.position);
            else
                agent.SetDestination(currentTarget.position);
        }
    }

    float GetDistanceToTarget()
    {
        if (currentTarget == null)
            return Mathf.Infinity;

        //Collider 기준으로 실제 표면 거리 계산
        if (currentTarget.TryGetComponent<Collider>(out var col))
        {
            Vector3 closest = col.ClosestPoint(transform.position);
            return Vector3.Distance(transform.position, closest);
        }
        else
        {
            return Vector3.Distance(transform.position, currentTarget.position);
        }
    }

    float GetAdjustedRange(Transform target)
    {
        float range = enemy.data.attackRange;

        // arrier는 살짝 줄이기 (예: 0.8배)
        //  조금 더 멀리 공격 가능 (예: 1.2배)
        if (target.CompareTag("Barrier"))
            range *= 0.6f;
        else if (target.CompareTag("Crystal"))
            range *= .4f;

        return range;
    }

    IEnumerator HandleAttack(float adjustedRange)
    {
        isAttacking = true;

        while (currentTarget != null)
        {
            float dist = GetDistanceToTarget();
            float range = GetAdjustedRange(currentTarget); // 매 프레임 다시 계산

            if (dist <= range)
            {
                if (currentTarget.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(enemy.data.damage);
                    Debug.Log($"{currentTarget.name}에게 {enemy.data.damage} 피해!");
                }

                yield return new WaitForSeconds(enemy.data.attackCoolDown);
            }
            else
            {
                break;
            }
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
            temp.transform.SetParent(target);

            Destroy(temp, 1f); // 메모리 누적 방지
            return temp.transform;
        }

        return target;
    }

    private void HandleBarrierDestroyed(Barrier destroyedBarrier)
    {
        if (currentTarget != null && destroyedBarrier.transform == currentTarget)
        {
            currentTarget = null;
            UpdateTarget();
        }
    }
}
