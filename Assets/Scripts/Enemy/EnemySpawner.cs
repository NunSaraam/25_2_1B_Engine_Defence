using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform crystal;               // ��ǥ�� (ũ����Ż)
    public int enemiesPerWave = 3;
    public float timeBetweenWaves = 10f;
    public float spawnRadius = 15f;

    public bool autoStart = true;
    public float spawnInterval = 1.0f;

    private int currentWave = 0;
    private bool isSpawning = false;

    private void Start()
    {
        if (autoStart)
            StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(2f); // ���� �� ���

        while (true)
        {
            currentWave++;
            Debug.Log($"Wave {currentWave} ����!");

            yield return StartCoroutine(SpawnWaveEnemies());

            Debug.Log($"Wave {currentWave} �Ϸ�. ���� ���̺���� {timeBetweenWaves}�� ���...");
            yield return new WaitForSeconds(timeBetweenWaves);

            // ���̺꺰 ���̵� ���� (���� ���ϰ�)
            enemiesPerWave += 1;
        }
    }

    IEnumerator SpawnWaveEnemies()
    {
        isSpawning = true;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        Vector3 randomDir = Random.insideUnitSphere * spawnRadius;
        randomDir.y = 0f;
        Vector3 spawnPos = transform.position + randomDir;

        if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            GameObject enemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null && crystal != null)
            {
                ai.crystal = crystal;
            }
        }
        else
        {
            Debug.LogWarning("�� ���� ��ġ�� ã�� ���߽��ϴ�!");
        }
    }
}
