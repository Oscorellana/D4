using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject upgradePanel; // root panel (inactive by default)

    public Button choiceButtonA;
    public TMP_Text choiceA_Title;
    public TMP_Text choiceA_Desc;
    public Image choiceA_Icon;

    public Button choiceButtonB;
    public TMP_Text choiceB_Title;
    public TMP_Text choiceB_Desc;
    public Image choiceB_Icon;

    [Header("References")]
    public SpawnManager spawnManager;
    public PlayerUpgrade playerUpgrade;
    public PlayerController playerController;
    public Weapon playerWeapon;

    [Header("Optional Icons")]
    public Sprite iconCooldown;
    public Sprite iconDamage;
    public Sprite iconSpeed;
    public Sprite iconHealth;

    List<PlayerUpgrade.UpgradeType> pool = new List<PlayerUpgrade.UpgradeType> {
        PlayerUpgrade.UpgradeType.ShotCooldown,
        PlayerUpgrade.UpgradeType.Damage,
        PlayerUpgrade.UpgradeType.Speed,
        PlayerUpgrade.UpgradeType.Health
    };

    void Start()
    {
        // auto-find missing references so inspector omissions won't crash
        if (playerUpgrade == null) playerUpgrade = FindFirstObjectByType<PlayerUpgrade>();
        if (playerController == null) playerController = FindFirstObjectByType<PlayerController>();
        if (playerWeapon == null) playerWeapon = FindFirstObjectByType<Weapon>();
        if (spawnManager == null) spawnManager = FindFirstObjectByType<SpawnManager>();

        if (upgradePanel != null) upgradePanel.SetActive(false);

        // safety: ensure buttons exist before adding listeners (we'll assign listeners at runtime)
        if (choiceButtonA != null) choiceButtonA.onClick.RemoveAllListeners();
        if (choiceButtonB != null) choiceButtonB.onClick.RemoveAllListeners();
    }

    // Called by SpawnManager when wave clears
    public void ShowUpgradeChoices()
    {
        if (upgradePanel == null)
        {
            Debug.LogError("UpgradeUIManager: upgradePanel not assigned!");
            return;
        }
        upgradePanel.SetActive(true);

        // unlock cursor & show
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // disable player control & weapon input (safe)
        if (playerController != null) playerController.enabled = false;
        if (playerWeapon != null) playerWeapon.enabled = false;

        // pick two unique upgrades
        var temp = new List<PlayerUpgrade.UpgradeType>(pool);
        var a = temp[Random.Range(0, temp.Count)];
        temp.Remove(a);
        var b = temp[Random.Range(0, temp.Count)];

        SetupButton(choiceButtonA, a, choiceA_Title, choiceA_Desc, choiceA_Icon);
        SetupButton(choiceButtonB, b, choiceB_Title, choiceB_Desc, choiceB_Icon);
    }

    void SetupButton(Button btn, PlayerUpgrade.UpgradeType type, TMP_Text title, TMP_Text desc, Image icon)
    {
        if (btn == null)
        {
            Debug.LogError("UpgradeUIManager.SetupButton: btn is null");
            return;
        }

        if (title == null || desc == null)
        {
            Debug.LogError("UpgradeUIManager.SetupButton: Text fields are null for " + btn.name);
            return;
        }

        // set text + description + icon (icon optional)
        title.text = PrettyName(type);
        desc.text = Description(type);
        if (icon != null) icon.sprite = IconFor(type);

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => OnSelected(type));
    }

    void OnSelected(PlayerUpgrade.UpgradeType chosen)
    {
        if (playerUpgrade == null)
            playerUpgrade = FindFirstObjectByType<PlayerUpgrade>();

        if (playerUpgrade != null)
            playerUpgrade.ApplyUpgrade(chosen);
        else
            Debug.LogWarning("UpgradeUIManager: PlayerUpgrade not found to apply upgrade.");

        // hide UI
        if (upgradePanel != null) upgradePanel.SetActive(false);

        // re-enable player control and weapon
        if (playerController != null) playerController.enabled = true;
        if (playerWeapon != null) playerWeapon.enabled = true;

        // lock cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // tell spawn manager to begin next wave
        if (spawnManager != null) spawnManager.AdvanceWaveAndStart();
        else Debug.LogWarning("UpgradeUIManager: spawnManager not assigned.");
    }

    string PrettyName(PlayerUpgrade.UpgradeType t)
    {
        switch (t)
        {
            case PlayerUpgrade.UpgradeType.ShotCooldown: return "Faster Fire";
            case PlayerUpgrade.UpgradeType.Damage: return "More Damage";
            case PlayerUpgrade.UpgradeType.Speed: return "Move Faster";
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
            case PlayerUpgrade.UpgradeType.Speed: return "Increase base movement speed.";
            case PlayerUpgrade.UpgradeType.Health: return "Increase maximum health.";
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
