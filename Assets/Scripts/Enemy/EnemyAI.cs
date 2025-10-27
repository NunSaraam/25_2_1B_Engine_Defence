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
        float adjustedRange = GetAdjustedRange(currentTarget); //����� �Ÿ� �Լ� ���

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

        // Barrier �켱 ����
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

        // ���� ����Ʈ ����
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

        //Collider �������� ���� ǥ�� �Ÿ� ���
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

        // arrier�� ��¦ ���̱� (��: 0.8��)
        //  ���� �� �ָ� ���� ���� (��: 1.2��)
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
            float range = GetAdjustedRange(currentTarget); // �� ������ �ٽ� ���

            if (dist <= range)
            {
                if (currentTarget.TryGetComponent<IDamageable>(out var target))
                {
                    target.TakeDamage(enemy.data.damage);
                    Debug.Log($"{currentTarget.name}���� {enemy.data.damage} ����!");
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

            Destroy(temp, 1f); // �޸� ���� ����
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
