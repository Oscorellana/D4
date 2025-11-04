using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int startingWave = 1;
    public int enemiesPerWave = 5;
    public int waveIncrement = 2; // Increase enemies per wave
    public float timeBetweenWaves = 3f;

    [Header("Spawning Settings")]
    public GameObject dummyPrefab;
    public Transform[] spawnPoints;

    [Header("UI (Optional)")]
    public Text waveText;

    private int currentWave;
    private int aliveEnemies = 0;
    private bool waveInProgress = false;

    void Start()
    {
        if (dummyPrefab == null)
        {
            Debug.LogError("SpawnManager: Dummy prefab is not assigned!");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager: No spawn points assigned!");
            return;
        }

        currentWave = startingWave;
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        waveInProgress = false;

        if (waveText != null)
            waveText.text = $"Wave {currentWave}";

        Debug.Log($"WaveManager: Wave {currentWave} will start in {timeBetweenWaves} seconds...");
        yield return new WaitForSeconds(timeBetweenWaves);

        waveInProgress = true;
        aliveEnemies = enemiesPerWave;

        Debug.Log($"WaveManager: Starting Wave {currentWave} with {enemiesPerWave} enemies.");

        // Spawn all enemies for this wave
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnDummy();
        }
    }

    public void SpawnDummy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject dummy = Instantiate(dummyPrefab, spawnPoint.position, spawnPoint.rotation);

        TargetDummy target = dummy.GetComponent<TargetDummy>();
        if (target != null)
        {
            target.OnDeath += () => DummyDied();
        }

        Debug.Log("Spawned dummy at " + spawnPoint.position);
    }

    private void DummyDied()
    {
        aliveEnemies--;

        if (aliveEnemies <= 0 && waveInProgress)
        {
            Debug.Log($"Wave {currentWave} cleared!");
            waveInProgress = false;
            currentWave++;
            enemiesPerWave += waveIncrement; // increase difficulty
            StartCoroutine(StartNextWave());
        }
    }
}
