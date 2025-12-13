using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject panel;

    public Button buttonA;
    public TMP_Text aTitle;
    public TMP_Text aMap;

    public Button buttonB;
    public TMP_Text bTitle;
    public TMP_Text bMap;

    PlayerUpgrade upgrades;
    SpawnManager spawner;

    PlayerUpgrade.UpgradeType upgradeA;
    PlayerUpgrade.UpgradeType upgradeB;
    MapData mapA;
    MapData mapB;

    void Start()
    {
        upgrades = FindFirstObjectByType<PlayerUpgrade>();
        spawner = FindFirstObjectByType<SpawnManager>();
        panel.SetActive(false);
    }

    public void ShowUpgradeChoices()
    {
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        upgradeA = RandomUpgrade();
        upgradeB = RandomUpgradeDifferent(upgradeA);

        mapA = MapManager.Instance.GetRandomMap(MapManager.Instance.currentMap);
        mapB = MapManager.Instance.GetRandomMap(mapA);

        aTitle.text = upgradeA.ToString();
        bTitle.text = upgradeB.ToString();

        aMap.text = mapA.mapName;
        bMap.text = mapB.mapName;

        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();

        buttonA.onClick.AddListener(() => Select(upgradeA, mapA));
        buttonB.onClick.AddListener(() => Select(upgradeB, mapB));
    }

    void Select(PlayerUpgrade.UpgradeType upgrade, MapData map)
    {
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        upgrades.ApplyUpgrade(upgrade);
        MapManager.Instance.TeleportPlayerToMap(map);
        spawner.StartWave();
    }

    PlayerUpgrade.UpgradeType RandomUpgrade()
    {
        return (PlayerUpgrade.UpgradeType)Random.Range(0, 4);
    }

    PlayerUpgrade.UpgradeType RandomUpgradeDifferent(PlayerUpgrade.UpgradeType other)
    {
        PlayerUpgrade.UpgradeType u;
        do { u = RandomUpgrade(); }
        while (u == other);
        return u;
    }
}
