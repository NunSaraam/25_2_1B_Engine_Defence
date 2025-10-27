using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform crystal;               // 목표물 (크리스탈)
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
        yield return new WaitForSeconds(2f); // 시작 전 대기

        while (true)
        {
            currentWave++;
            Debug.Log($"Wave {currentWave} 시작!");

            yield return StartCoroutine(SpawnWaveEnemies());

            Debug.Log($"Wave {currentWave} 완료. 다음 웨이브까지 {timeBetweenWaves}초 대기...");
            yield return new WaitForSeconds(timeBetweenWaves);

            // 웨이브별 난이도 조정 (점점 강하게)
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
            Debug.LogWarning("적 스폰 위치를 찾지 못했습니다!");
        }
    }
}
