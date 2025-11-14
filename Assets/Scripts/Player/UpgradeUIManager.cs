using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject upgradePanel;
    public Button upgradeButton1;
    public Button upgradeButton2;
    public TMP_Text upgradeText1;
    public TMP_Text upgradeText2;

    [Header("References")]
    public SpawnManager spawnManager;
    public PlayerUpgrade playerUpgrade;

    private string upgrade1;
    private string upgrade2;

    void Start()
    {
        upgradePanel.SetActive(false);
    }

    public void ShowUpgrades()
    {
        upgradePanel.SetActive(true);

        // Randomly pick 2 of the 4 upgrades
        string[] allUpgrades = { "Cooldown", "Damage", "Speed", "Health" };
        upgrade1 = allUpgrades[Random.Range(0, allUpgrades.Length)];
        do
        {
            upgrade2 = allUpgrades[Random.Range(0, allUpgrades.Length)];
        } while (upgrade2 == upgrade1);

        // Update button text
        upgradeText1.text = upgrade1;
        upgradeText2.text = upgrade2;

        // Reset button listeners
        upgradeButton1.onClick.RemoveAllListeners();
        upgradeButton2.onClick.RemoveAllListeners();

        // Add listeners that call ApplyUpgrade and resume next wave
        upgradeButton1.onClick.AddListener(() => SelectUpgrade(upgrade1));
        upgradeButton2.onClick.AddListener(() => SelectUpgrade(upgrade2));
    }

    public void SelectUpgrade(string chosenUpgrade)
    {
        playerUpgrade.ApplyUpgrade(chosenUpgrade);
        upgradePanel.SetActive(false);
        spawnManager.StartNextWave();
    }
}
