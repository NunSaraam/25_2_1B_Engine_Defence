using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPositionManager : MonoBehaviour
{
    public Transform[] attackPoints;
    private bool[] occupied;

    private void Start()
    {
        if (attackPoints == null || attackPoints.Length == 0)
        {
            Debug.LogWarning($"{name}: AttackPoints가 비어 있음!");
            return;
        }

        occupied = new bool[attackPoints.Length];
    }

    public Transform GetNearestAvailablePoint(Vector3 fromPos)
    {
        int bestIndex = -1;
        float bestDist = float.MaxValue;

        for (int i = 0; i < attackPoints.Length; i++)
        {
            if (attackPoints[i] == null) continue;
            if (occupied[i]) continue;

            float dist = Vector3.Distance(fromPos, attackPoints[i].position);
            if (dist < bestDist)
            {
                bestDist = dist;
                bestIndex = i;
            }
        }

        if (bestIndex >= 0)
        {
            occupied[bestIndex] = true;
            return attackPoints[bestIndex];
        }

        // 모든 포인트가 점유된 경우 null 반환
        Debug.Log($"{name}: 모든 공격 포인트가 점유 중 -> Random 포인트 사용");
        return null;
    }

    public void ReleasePoint(Transform point)
    {
        for (int i = 0; i < attackPoints.Length; i++)
        {
            if (attackPoints[i] == point)
            {
                occupied[i] = false;
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoints == null) return;
        Gizmos.color = Color.green;
        foreach (var p in attackPoints)
        {
            if (p != null)
                Gizmos.DrawSphere(p.position, 0.2f);
        }
    }
}
