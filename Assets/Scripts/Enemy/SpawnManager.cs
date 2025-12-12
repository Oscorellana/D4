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
    public Transform[] spawnPoints; // <-- assign these in Inspector

    [Header("References")]
    public UpgradeUIManager upgradeUIManager; // assign your UI manager (optional)

    private int currentWave;
    private int aliveEnemies;
    private bool waveInProgress = false;

    void Start()
    {
        // Safety checks
        if (enemyPrefab == null)
        {
            Debug.LogError("SpawnManager: enemyPrefab is not assigned!");
            enabled = false;
            return;
        }

        // If no spawnPoints assigned, auto-create some around the Player
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnManager: No spawnPoints assigned, creating fallback spawn points.");
            CreateFallbackSpawnPoints(6, 6f);
        }

        currentWave = startingWave;
        StartCoroutine(StartNextWaveRoutine());
    }

    // Create fallback empty transforms around the player if user forgot to assign them.
    void CreateFallbackSpawnPoints(int count, float radius)
    {
        List<Transform> pts = new List<Transform>();
        Transform player = GameObject.FindWithTag("Player")?.transform;
        Vector3 center = player != null ? player.position : Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject($"SpawnPoint_{i}");
            go.transform.position = center + new Vector3(
                Random.Range(-radius, radius),
                0f,
                Random.Range(-radius, radius)
            );
            go.transform.parent = this.transform;
            pts.Add(go.transform);
        }

        spawnPoints = pts.ToArray();
    }

    public void StartNextWave()
    {
        StartCoroutine(StartNextWaveRoutine());
    }

    private IEnumerator StartNextWaveRoutine()
    {
        waveInProgress = false;
        if (waveInProgress == false)
            Debug.Log($"SpawnManager: Preparing wave {currentWave}...");

        yield return new WaitForSeconds(timeBetweenWaves);

        waveInProgress = true;
        aliveEnemies = enemiesPerWave;
        Debug.Log($"SpawnManager: Starting Wave {currentWave} with {enemiesPerWave} enemies.");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemyAtRandomPoint();
            yield return new WaitForSeconds(spawnDelayBetweenEnemies);
        }
    }

    void SpawnEnemyAtRandomPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager: No spawn points available to spawn enemies.");
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

            // Show upgrade UI if present (does NOT pause time)
            if (upgradeUIManager != null)
            {
                upgradeUIManager.ShowUpgradeChoices();
            }
            else
            {
                // If no UI manager, immediately advance wave
                AdvanceWaveAndStart();
            }
        }
    }

    // Called by UpgradeUIManager when upgrade finished
    public void AdvanceWaveAndStart()
    {
        currentWave++;
        enemiesPerWave += waveIncrement;
        StartCoroutine(StartNextWaveRoutine());
    }
}
