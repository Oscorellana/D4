using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Wave Settings")]
    public int enemiesPerWave = 5;
    public int waveIncrement = 2;
    public float spawnDelay = 0.25f;

    [Header("UI")]
    public UpgradeUIManager upgradeUI;

    int aliveEnemies;
    int globalWave = 1;
    int wavesThisMap = 0;

    // ✅ Public read-only access for UI
    public int GlobalWave => globalWave;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ResetForNewMap()
    {
        wavesThisMap = 0;
    }

    public void StartNextWave()
    {
        Debug.Log($"Starting Global Wave {globalWave} | Map Wave {wavesThisMap + 1}");

        // ✅ Update Wave UI when wave starts
        if (WaveUI.Instance != null)
            WaveUI.Instance.UpdateWaveText(globalWave);

        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        yield return new WaitForSeconds(1f);

        wavesThisMap++;
        aliveEnemies = enemiesPerWave;

        MapData map = MapManager.Instance.currentMap;

        if (map == null || map.enemySpawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager: Invalid map or missing enemy spawn points!");
            yield break;
        }

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Transform sp = map.enemySpawnPoints[Random.Range(0, map.enemySpawnPoints.Length)];

            GameObject enemy = Instantiate(
                map.enemyPrefab,
                sp.position,
                sp.rotation
            );

            TargetDummy dummy = enemy.GetComponent<TargetDummy>();
            if (dummy != null)
                dummy.OnDeath += OnEnemyDeath;

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void OnEnemyDeath()
    {
        aliveEnemies--;

        if (aliveEnemies > 0)
            return;

        enemiesPerWave += waveIncrement;
        globalWave++;

        if (wavesThisMap >= 3)
        {
            Debug.Log("Opening upgrade panel");
            upgradeUI.ShowUpgradeChoices();
        }
        else
        {
            StartNextWave();
        }
    }
}
