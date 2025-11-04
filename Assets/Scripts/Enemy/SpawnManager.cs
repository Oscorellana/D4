using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int startingWave = 1;
    public int enemiesPerWave = 5;
    public int waveIncrement = 2;
    public float timeBetweenWaves = 3f;

    [Header("Spawning Settings")]
    public GameObject dummyPrefab;
    public Transform[] spawnPoints;

    [Header("UI Settings")]
    public Text waveText;
    public GameObject upgradePanel; // assign a panel with buttons for upgrades

    private int currentWave;
    private int aliveEnemies = 0;
    private bool waveInProgress = false;

    void Start()
    {
        if (dummyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager: Missing dummy prefab or spawn points!");
            return;
        }

        currentWave = startingWave;
        upgradePanel?.SetActive(false);
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        waveInProgress = false;

        if (waveText != null)
            waveText.text = $"Wave {currentWave}";

        Debug.Log($"WaveManager: Wave {currentWave} starting in {timeBetweenWaves} seconds...");
        yield return new WaitForSeconds(timeBetweenWaves);

        waveInProgress = true;
        aliveEnemies = enemiesPerWave;

        Debug.Log($"WaveManager: Starting Wave {currentWave} with {enemiesPerWave} enemies.");

        for (int i = 0; i < enemiesPerWave; i++)
            SpawnDummy();
    }

    public void SpawnDummy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject dummy = Instantiate(dummyPrefab, spawnPoint.position, spawnPoint.rotation);

        TargetDummy target = dummy.GetComponent<TargetDummy>();
        if (target != null)
            target.OnDeath += () => DummyDied();

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
            enemiesPerWave += waveIncrement;

            // Start upgrade phase
            StartCoroutine(StartUpgradePhase());
        }
    }

    IEnumerator StartUpgradePhase()
    {
        Debug.Log("Upgrade Phase: Player can choose upgrades now!");
        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        // Pause spawning and wait for player input
        while (upgradePanel != null && upgradePanel.activeSelf)
        {
            yield return null; // wait until player closes the upgrade panel
        }

        // Start next wave
        StartCoroutine(StartNextWave());
    }

    // Call this from a button on the upgrade UI
    public void FinishUpgrades()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        Debug.Log("Player finished upgrades, next wave starting...");
    }
}
