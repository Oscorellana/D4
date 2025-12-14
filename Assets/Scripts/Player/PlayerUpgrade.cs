using UnityEngine;
using System.Collections.Generic;

public class PlayerUpgrade : MonoBehaviour
{
    public enum UpgradeType
    {
        ShotCooldown,
        Damage,
        Speed,
        Health
    }

    [Header("Core Stats")]
    public int damage = 1;
    public float fireRate = 0.25f;
    public float moveSpeedBonus = 0f;

    [Header("References")]
    public PlayerController playerController;
    public PlayerHealth playerHealth;
    public Weapon weapon;

    List<UpgradeType> upgradePool = new List<UpgradeType>
    {
        UpgradeType.ShotCooldown,
        UpgradeType.Damage,
        UpgradeType.Speed,
        UpgradeType.Health
    };

    void Awake()
    {
        if (playerController == null)
            playerController = FindFirstObjectByType<PlayerController>();

        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (weapon == null)
            weapon = FindFirstObjectByType<Weapon>();
    }

    // ------------------------
    // APPLY UPGRADE
    // ------------------------

    public void ApplyUpgrade(UpgradeType upgrade)
    {
        Debug.Log($"Applying Upgrade: {upgrade}");

        switch (upgrade)
        {
            case UpgradeType.Damage:
                damage += 1;
                break;

            case UpgradeType.ShotCooldown:
                fireRate = Mathf.Max(0.05f, fireRate * 0.85f);
                break;

            case UpgradeType.Speed:
                moveSpeedBonus += 1f;
                if (playerController != null)
                    playerController.walkSpeed += 1f;
                break;

            case UpgradeType.Health:
                if (playerHealth != null)
                    playerHealth.AddMaxHealth(20f);
                break;
        }
    }

    // ------------------------
    // UI HELPERS
    // ------------------------

    public UpgradeType GetRandomUpgrade()
    {
        return upgradePool[Random.Range(0, upgradePool.Count)];
    }

    public string GetDescription(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.Damage:
                return "+1 Bullet Damage";

            case UpgradeType.ShotCooldown:
                return "Shoot Faster";

            case UpgradeType.Speed:
                return "Move Faster";

            case UpgradeType.Health:
                return "+20 Max Health";

            default:
                return "";
        }
    }

    // Overload for UpgradeUIManager compatibility
    public UpgradeType GetRandomUpgrade(MapData map)
    {
        // Map-based filtering can be added later
        return GetRandomUpgrade();
    }


}
