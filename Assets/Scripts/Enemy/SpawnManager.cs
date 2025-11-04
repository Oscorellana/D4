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
    public float timeBetweenWaves = 3f;

    [Header("Spawning Settings")]
    public GameObject dummyPrefab;
    public Transform[] spawnPoints;

    [Header("UI Settings")]
    public Text waveText;
    public GameObject upgradePanel; // panel that contains upgrade buttons
    public Button[] upgradeButtons; // assign 4 buttons (only two will be shown each wave)

    [Header("References")]
    public PlayerUpgrade playerUpgrade; // reference to PlayerUpgrade on player

    private int currentWave;
    private int aliveEnemies = 0;
    private bool waveInProgress = false;
    private readonly string[] allUpgrades = { "FireRate", "Damage", "Speed", "Health" };

    void Start()
    {
        if (dummyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnManager: Missing setup (dummyPrefab or spawnPoints).");
            return;
        }

        currentWave = startingWave;
        if (upgradePanel != null) upgradePanel.SetActive(false);
        StartCoroutine(StartNextWaveRoutine());
    }

    // Public friendly wrapper so UI scripts can call it
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

        for (int i = 0; i < enemiesPerWave; i++) SpawnDummy();
    }

    public void SpawnDummy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject dummy = Instantiate(dummyPrefab, spawnPoint.position, spawnPoint.rotation);
        TargetDummy td = dummy.GetComponent<TargetDummy>();
        if (td != null) td.OnDeath += DummyDied;
        Debug.Log($"Spawned dummy at {spawnPoint.position}");
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
            StartCoroutine(StartUpgradePhase());
        }
    }

    private IEnumerator StartUpgradePhase()
    {
        if (upgradePanel != null) upgradePanel.SetActive(true);
        SetupUpgradeButtons();

        // Wait until upgrade panel is closed (FinishUpgrades will deactivate)
        while (upgradePanel != null && upgradePanel.activeSelf) yield return null;

        StartCoroutine(StartNextWaveRoutine());
    }

    private void SetupUpgradeButtons()
    {
        // hide all buttons first
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            var b = upgradeButtons[i];
            b.gameObject.SetActive(false);
            b.onClick.RemoveAllListeners();
        }

        // pick two unique upgrades
        List<string> pool = new List<string>(allUpgrades);
        List<string> chosen = new List<string>();
        while (chosen.Count < 2 && pool.Count > 0)
        {
            int idx = Random.Range(0, pool.Count);
            chosen.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        for (int i = 0; i < chosen.Count && i < upgradeButtons.Length; i++)
        {
            string up = chosen[i];
            Button btn = upgradeButtons[i];
            btn.gameObject.SetActive(true);
            Text txt = btn.GetComponentInChildren<Text>();
            if (txt != null) txt.text = GetPrettyName(up);

            btn.onClick.AddListener(() =>
            {
                playerUpgrade.ApplyUpgrade(up);
                FinishUpgrades();
            });
        }
    }

    static string GetPrettyName(string key)
    {
        switch (key)
        {
            case "FireRate": return "Faster Fire";
            case "Damage": return "More Damage";
            case "Speed": return "Move Faster";
            case "Health": return "Increase Health";
            default: return key;
        }
    }

    public void FinishUpgrades()
    {
        if (upgradePanel != null) upgradePanel.SetActive(false);
        Debug.Log("Player finished upgrades, next wave starting...");
    }
}
