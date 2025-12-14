using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject panel;

    public Button optionButtonA;
    public Button optionButtonB;

    public TextMeshProUGUI optionTextA;
    public TextMeshProUGUI optionTextB;

    PlayerUpgrade playerUpgrade;

    PlayerUpgrade.UpgradeType upgradeA;
    PlayerUpgrade.UpgradeType upgradeB;

    void Awake()
    {
        playerUpgrade = FindAnyObjectByType<PlayerUpgrade>();

        panel.SetActive(false);
    }

    public void ShowUpgradeChoices()
    {
        Time.timeScale = 0f;
        panel.SetActive(true);

        // 🔹 CORRECT: no arguments passed
        upgradeA = playerUpgrade.GetRandomUpgrade();
        upgradeB = playerUpgrade.GetRandomUpgrade();

        // prevent duplicates
        while (upgradeB == upgradeA)
            upgradeB = playerUpgrade.GetRandomUpgrade();

        optionTextA.text = playerUpgrade.GetDescription(upgradeA);
        optionTextB.text = playerUpgrade.GetDescription(upgradeB);

        optionButtonA.onClick.RemoveAllListeners();
        optionButtonB.onClick.RemoveAllListeners();

        optionButtonA.onClick.AddListener(() => Select(upgradeA));
        optionButtonB.onClick.AddListener(() => Select(upgradeB));
    }

    void Select(PlayerUpgrade.UpgradeType upgrade)
    {
        panel.SetActive(false);
        Time.timeScale = 1f;

        playerUpgrade.ApplyUpgrade(upgrade);

        // Resume game
        SpawnManager.Instance.StartNextWave();
    }
}
