using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/NewEnemy", fileName = "Enemy")]
public class EnemySO : ScriptableObject
{
    public string enemyName = "Swarm";
    public GameObject enemyPrefab;

    public float enemyHealth = 10f;
    public float moveSpeed = 3f;
    public float damage = 5f;

}
