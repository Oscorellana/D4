using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int startingWave = 1;
    public int enemiesPerWave = 5;
    public int waveIncrement = 2;
    public float timeBetweenWaves = 2f;
    public float spawnDelayBetweenEnemies = 0.25f;

    [Header("Spawning Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("References")]
    public UpgradeUIManager upgradeUIManager;

    int currentWave;
    int aliveEnemies;
    bool waveInProgress = false;

    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("SpawnManager: enemyPrefab not assigned!");
            enabled = false;
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnManager: spawnPoints not assigned. Creating fallback points.");
            CreateFallbackSpawnPoints(6, 6f);
        }

        currentWave = startingWave;
        StartCoroutine(StartNextWaveRoutine());
    }

    public void StartNextWave()
    {
        StartCoroutine(StartNextWaveRoutine());
    }

    IEnumerator StartNextWaveRoutine()
    {
        waveInProgress = false;
        if (waveInProgress == false) Debug.Log($"SpawnManager: Preparing wave {currentWave}...");
        yield return new WaitForSeconds(timeBetweenWaves);

        waveInProgress = true;
        aliveEnemies = enemiesPerWave;
        Debug.Log($"SpawnManager: Starting Wave {currentWave} with {enemiesPerWave} enemies.");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelayBetweenEnemies);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager: No spawnPoints available.");
            return;
        }

        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject e = Instantiate(enemyPrefab, sp.position, sp.rotation);
        TargetDummy td = e.GetComponent<TargetDummy>();
        if (td != null) td.OnDeath += OnEnemyDeath;
        else Debug.LogWarning("Spawned enemy missing TargetDummy component.");
        Debug.Log("Spawned enemy at " + sp.position);
    }

    public void OnEnemyDeath()
    {
        aliveEnemies--;
        if (aliveEnemies <= 0 && waveInProgress)
        {
            Debug.Log($"Wave {currentWave} cleared!");
            waveInProgress = false;

            if (upgradeUIManager != null) upgradeUIManager.ShowUpgradeChoices();
            else AdvanceWaveAndStart();
        }
    }

    public void AdvanceWaveAndStart()
    {
        currentWave++;
        enemiesPerWave += waveIncrement;
        StartCoroutine(StartNextWaveRoutine());
    }

    void CreateFallbackSpawnPoints(int count, float radius)
    {
        var pts = new List<Transform>();
        Transform player = GameObject.FindWithTag("Player")?.transform;
        Vector3 center = player != null ? player.position : Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject($"SpawnPoint_{i}");
            go.transform.position = center + new Vector3(Random.Range(-radius, radius), 0f, Random.Range(-radius, radius));
            go.transform.parent = transform;
            pts.Add(go.transform);
        }
        spawnPoints = pts.ToArray();
    }
}
