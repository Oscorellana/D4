using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject upgradePanel;     // panel root (inactive by default)
    public Button choiceButtonA;        // slot A
    public Button choiceButtonB;        // slot B
    public TMP_Text choiceA_Title;
    public TMP_Text choiceA_Desc;
    public Image choiceA_Icon;
    public TMP_Text choiceB_Title;
    public TMP_Text choiceB_Desc;
    public Image choiceB_Icon;

    [Header("References")]
    public SpawnManager spawnManager;
    public PlayerUpgrade playerUpgrade;

    // small icon map (optional) — assign sprites in Inspector
    public Sprite iconCooldown;
    public Sprite iconDamage;
    public Sprite iconSpeed;
    public Sprite iconHealth;

    private List<PlayerUpgrade.UpgradeType> pool = new List<PlayerUpgrade.UpgradeType> {
        PlayerUpgrade.UpgradeType.ShotCooldown,
        PlayerUpgrade.UpgradeType.Damage,
        PlayerUpgrade.UpgradeType.Speed,
        PlayerUpgrade.UpgradeType.Health
    };

    void Start()
    {
        if (upgradePanel != null) upgradePanel.SetActive(false);
    }

    public void ShowUpgradeChoices()
    {
        if (upgradePanel != null) upgradePanel.SetActive(true);
        if (playerUpgrade == null) playerUpgrade = FindFirstObjectByType<PlayerUpgrade>();

        // choose two unique upgrades
        List<PlayerUpgrade.UpgradeType> temp = new List<PlayerUpgrade.UpgradeType>(pool);
        PlayerUpgrade.UpgradeType a = temp[Random.Range(0, temp.Count)];
        temp.Remove(a);
        PlayerUpgrade.UpgradeType b = temp[Random.Range(0, temp.Count)];

        SetupButton(choiceButtonA, a, choiceA_Title, choiceA_Desc, choiceA_Icon);
        SetupButton(choiceButtonB, b, choiceB_Title, choiceB_Desc, choiceB_Icon);
    }

    void SetupButton(Button btn, PlayerUpgrade.UpgradeType type, TMP_Text title, TMP_Text desc, Image icon)
    {
        if (btn == null) return;

        // set text + description + icon
        title.text = PrettyName(type);
        desc.text = Description(type);
        icon.sprite = IconFor(type);

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => { OnSelected(type); });
    }

    void OnSelected(PlayerUpgrade.UpgradeType chosen)
    {
        playerUpgrade.ApplyUpgrade(chosen);

        // hide UI and resume waves
        if (upgradePanel != null) upgradePanel.SetActive(false);
        if (spawnManager != null) spawnManager.ResumeAfterUpgrade();
    }

    string PrettyName(PlayerUpgrade.UpgradeType t)
    {
        switch (t)
        {
            case PlayerUpgrade.UpgradeType.ShotCooldown: return "Faster Fire";
            case PlayerUpgrade.UpgradeType.Damage: return "More Damage";
            case PlayerUpgrade.UpgradeType.Speed: return "Increased Speed";
            case PlayerUpgrade.UpgradeType.Health: return "Max Health +20";
            default: return t.ToString();
        }
    }

    string Description(PlayerUpgrade.UpgradeType t)
    {
        switch (t)
        {
            case PlayerUpgrade.UpgradeType.ShotCooldown: return "Decrease time between shots.";
            case PlayerUpgrade.UpgradeType.Damage: return "Increase bullet damage.";
            case PlayerUpgrade.UpgradeType.Speed: return "Move faster when walking.";
            case PlayerUpgrade.UpgradeType.Health: return "Increase your maximum health.";
            default: return "";
        }
    }

    Sprite IconFor(PlayerUpgrade.UpgradeType t)
    {
        switch (t)
        {
            case PlayerUpgrade.UpgradeType.ShotCooldown: return iconCooldown;
            case PlayerUpgrade.UpgradeType.Damage: return iconDamage;
            case PlayerUpgrade.UpgradeType.Speed: return iconSpeed;
            case PlayerUpgrade.UpgradeType.Health: return iconHealth;
            default: return null;
        }
    }
}
