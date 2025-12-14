using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUIManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;

    public Button leftButton;
    public Button rightButton;

    public TMP_Text leftTitle;
    public TMP_Text leftDesc;
    public TMP_Text rightTitle;
    public TMP_Text rightDesc;

    public TMP_Text leftMapText;
    public TMP_Text rightMapText;

    PlayerUpgrade playerUpgrade;
    SpawnManager spawnManager;

    PlayerUpgrade.UpgradeType leftUpgrade;
    PlayerUpgrade.UpgradeType rightUpgrade;

    MapData leftMap;
    MapData rightMap;

    void Awake()
    {
        playerUpgrade = FindFirstObjectByType<PlayerUpgrade>();
        spawnManager = FindFirstObjectByType<SpawnManager>();

        panel.SetActive(false);
    }

    // ====================================
    // CALLED AFTER 3 WAVES
    // ====================================
    public void ShowUpgradeChoices()
    {
        panel.SetActive(true);

        // Pause gameplay
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();

        // Pick upgrades
        leftUpgrade = playerUpgrade.GetRandomUpgrade();
        rightUpgrade = playerUpgrade.GetRandomUpgrade();

        // Pick maps (NO ARGUMENTS — matches your MapManager)
        leftMap = MapManager.Instance.GetRandomMap();
        rightMap = MapManager.Instance.GetRandomMap();

        // Fill UI
        leftTitle.text = leftUpgrade.ToString();
        rightTitle.text = rightUpgrade.ToString();

        leftDesc.text = playerUpgrade.GetDescription(leftUpgrade);
        rightDesc.text = playerUpgrade.GetDescription(rightUpgrade);

        leftMapText.text = leftMap.mapName;
        rightMapText.text = rightMap.mapName;

        leftButton.onClick.AddListener(() =>
        {
            Select(leftUpgrade, leftMap);
        });

        rightButton.onClick.AddListener(() =>
        {
            Select(rightUpgrade, rightMap);
        });
    }

    // ====================================
    // PLAYER MAKES A CHOICE
    // ====================================
    void Select(PlayerUpgrade.UpgradeType upgrade, MapData map)
    {
        playerUpgrade.ApplyUpgrade(upgrade);

        panel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MapManager.Instance.LoadMap(map);
        spawnManager.StartNextWave();
    }
}
