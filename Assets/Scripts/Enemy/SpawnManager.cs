using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int enemiesPerWave = 5;
    public int waveIncrement = 2;
    public float spawnDelay = 0.25f;

    [Header("References")]
    public GameObject enemyPrefab;
    public UpgradeUIManager upgradeUI;

    int aliveEnemies;
    int globalWave = 1;
    int wavesThisMap = 0;

    void Start()
    {
        StartWave();
    }

    public void StartWave()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        yield return new WaitForSeconds(1f);

        if (enemyPrefab == null)
        {
            Debug.LogError("SpawnManager: Enemy prefab is NOT assigned!");
            yield break;
        }

        if (MapManager.Instance == null || MapManager.Instance.currentMap == null)
        {
            Debug.LogError("SpawnManager: No current map set!");
            yield break;
        }

        Transform[] points = MapManager.Instance.currentMap.enemySpawnPoints;

        if (points == null || points.Length == 0)
        {
            Debug.LogError("SpawnManager: Current map has no enemy spawn points!");
            yield break;
        }

        wavesThisMap++;
        aliveEnemies = enemiesPerWave;

        Debug.Log($"Starting Wave {globalWave} | Map Wave {wavesThisMap}");

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Transform sp = points[Random.Range(0, points.Length)];
            GameObject e = Instantiate(enemyPrefab, sp.position, sp.rotation);

            TargetDummy td = e.GetComponent<TargetDummy>();
            if (td != null)
                td.OnDeath += OnEnemyDeath;
            else
                Debug.LogError("Enemy missing TargetDummy component!");

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void OnEnemyDeath()
    {
        aliveEnemies--;
        if (aliveEnemies > 0) return;

        enemiesPerWave += waveIncrement;
        globalWave++;

        if (wavesThisMap >= 3)
        {
            wavesThisMap = 0;
            if (upgradeUI != null)
                upgradeUI.ShowUpgradeChoices();
            else
                Debug.LogError("SpawnManager: UpgradeUI reference missing!");
        }
        else
        {
            StartWave();
        }
    }
}
