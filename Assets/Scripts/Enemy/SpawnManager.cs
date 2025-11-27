using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI Settings")]
    public Text waveText;
    public GameObject upgradePanel; // panel controlled by UpgradeUIManager

    [Header("References")]
    public UpgradeUIManager upgradeUIManager;
    public PlayerUpgrade playerUpgrade; // reference for upgrades (Inspector)

    private int currentWave;
    private int aliveEnemies;
    private bool waveInProgress = false;

    void Start()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager: missing enemyPrefab or spawnPoints");
            return;
        }

        if (upgradePanel != null) upgradePanel.SetActive(false);
        currentWave = startingWave;
        StartCoroutine(StartNextWaveRoutine());
    }

    public void StartNextWave()
    {
        StartCoroutine(StartNextWaveRoutine());
    }

    private IEnumerator StartNextWaveRoutine()
    {
        waveInProgress = false;
        if (waveText != null) waveText.text = $"Wave {currentWave}";
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
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject e = Instantiate(enemyPrefab, sp.position, sp.rotation);
        TargetDummy td = e.GetComponent<TargetDummy>();
        if (td != null) td.OnDeath += OnEnemyDeath;
        else Debug.LogWarning("Spawned enemy without TargetDummy");
        Debug.Log("Spawned enemy at " + sp.position);
    }

    void OnEnemyDeath()
    {
        aliveEnemies--;
        if (aliveEnemies <= 0 && waveInProgress)
        {
            Debug.Log($"Wave {currentWave} cleared!");
            waveInProgress = false;

            // start upgrade phase
            if (upgradeUIManager != null)
            {
                // pause gameplay
                Time.timeScale = 0f;
                upgradeUIManager.ShowUpgradeChoices();
            }
            else
            {
                // no UI manager -> start next wave automatically
                currentWave++;
                enemiesPerWave += waveIncrement;
                StartCoroutine(StartNextWaveRoutine());
            }
        }
    }

    // called by UI manager when upgrades finished
    public void ResumeAfterUpgrade()
    {
        // unpause and advance wave counters
        Time.timeScale = 1f;
        currentWave++;
        enemiesPerWave += waveIncrement;
        StartCoroutine(StartNextWaveRoutine());
    }
}
