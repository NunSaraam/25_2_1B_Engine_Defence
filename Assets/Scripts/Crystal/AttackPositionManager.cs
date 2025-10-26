using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPositionManager : MonoBehaviour
{
    public Transform[] attackPoints;
    private bool[] occupied;

    private void Awake()
    {
        occupied = new bool[attackPoints.Length];
    }

    public Transform GetNearestAvailablePoint(Vector3 fromPos)
    {
        int bestIndex = -1;
        float bestDist = float.MaxValue;

        for (int i = 0; i < attackPoints.Length; i++)
        {
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
}
