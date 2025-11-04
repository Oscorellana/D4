using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgrade : MonoBehaviour
{
    [Header("Player References")]
    public PlayerController playerController;
    public PlayerHealth playerHealth;
    public Weapon playerWeapon;

    [Header("UI References")]
    public GameObject upgradePanel; // assign same as SpawnManager.upgradePanel
    public Button upgradeButtonPrefab; // prefab for upgrade buttons
    public Transform buttonContainer;   // parent for buttons

    [Header("Upgrade Settings")]
    public int upgradesPerWave = 2; // how many choices per wave

    // All possible upgrades
    private enum UpgradeType { ShotCooldown, Damage, Speed, Health }
    private List<UpgradeType> allUpgrades = new List<UpgradeType> { UpgradeType.ShotCooldown, UpgradeType.Damage, UpgradeType.Speed, UpgradeType.Health };

    private List<Button> currentButtons = new List<Button>();

    /// <summary>
    /// Call this when the wave ends to show upgrade options.
    /// </summary>
    public void ShowUpgrades()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        ClearButtons();

        // Pick random upgrades for this wave
        List<UpgradeType> copy = new List<UpgradeType>(allUpgrades);
        for (int i = 0; i < upgradesPerWave; i++)
        {
            if (copy.Count == 0) break;

            int index = Random.Range(0, copy.Count);
            UpgradeType chosen = copy[index];
            copy.RemoveAt(index);

            CreateUpgradeButton(chosen);
        }
    }

    void CreateUpgradeButton(UpgradeType upgrade)
    {
        if (upgradeButtonPrefab == null || buttonContainer == null) return;

        Button button = Instantiate(upgradeButtonPrefab, buttonContainer);
        button.GetComponentInChildren<Text>().text = GetUpgradeName(upgrade);

        // Add listener
        button.onClick.AddListener(() =>
        {
            ApplyUpgrade(upgrade);
            // Optionally disable buttons after selection if you want only one upgrade at a time
            // button.interactable = false;
        });

        currentButtons.Add(button);
    }

    void ClearButtons()
    {
        foreach (Button b in currentButtons)
        {
            Destroy(b.gameObject);
        }
        currentButtons.Clear();
    }

    void ApplyUpgrade(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.ShotCooldown:
                if (playerWeapon != null)
                    playerWeapon.fireRate = Mathf.Max(0.05f, playerWeapon.fireRate - 0.05f); // decrease cooldown
                break;

            case UpgradeType.Damage:
                // assume Weapon or Bullet script has a damage property
                if (playerWeapon != null)
                {
                    Bullet bullet = playerWeapon.bulletPrefab.GetComponent<Bullet>();
                    if (bullet != null)
                        bullet.damage += 1;
                }
                break;

            case UpgradeType.Speed:
                if (playerController != null)
                    playerController.walkSpeed += 1f; // increment walk speed
                break;

            case UpgradeType.Health:
                if (playerHealth != null)
                {
                    playerHealth.maxHealth += 20f;
                    playerHealth.TakeDamage(-20f); // heal player to reflect new max
                }
                break;
        }

        Debug.Log($"Applied Upgrade: {GetUpgradeName(upgrade)}");
    }

    string GetUpgradeName(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.ShotCooldown: return "Faster Fire";
            case UpgradeType.Damage: return "More Damage";
            case UpgradeType.Speed: return "Move Faster";
            case UpgradeType.Health: return "Increase Health";
            default: return "Upgrade";
        }
    }

    /// <summary>
    /// Call this when the player finishes choosing upgrades.
    /// </summary>
    public void FinishUpgrades()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        ClearButtons();

        // Tell SpawnManager to start the next wave
        SpawnManager spawner = FindFirstObjectByType<SpawnManager>();
        if (spawner != null)
        {
            spawner.FinishUpgrades();
        }
    }
}
