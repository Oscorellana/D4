using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    public PlayerHealth playerHealth;
    public Weapon playerWeapon;

    [Header("Upgrade amounts")]
    public float damagePerUpgrade = 1f;
    public float fireRateMultiplier = 0.9f; // multiply fireRate by this (shrinks cooldown)
    public float speedPerUpgrade = 1f;
    public float healthPerUpgrade = 20f;

    void Start()
    {
        if (playerController == null) playerController = FindFirstObjectByType<PlayerController>();
        if (playerHealth == null) playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerWeapon == null) playerWeapon = FindFirstObjectByType<Weapon>();
    }

    // Public so UI/SpawnManager can call it
    public void ApplyUpgrade(string upgradeKey)
    {
        Debug.Log($"Applying upgrade: {upgradeKey}");
        switch (upgradeKey)
        {
            case "Damage":
                // modify bullet prefab's base damage if possible
                if (playerWeapon != null && playerWeapon.bulletPrefab != null)
                {
                    Bullet b = playerWeapon.bulletPrefab.GetComponent<Bullet>();
                    if (b != null) b.damage += Mathf.RoundToInt(damagePerUpgrade);
                }
                break;

            case "FireRate":
                if (playerWeapon != null) playerWeapon.fireRate = Mathf.Max(0.02f, playerWeapon.fireRate * fireRateMultiplier);
                break;

            case "Speed":
                if (playerController != null) playerController.walkSpeed += speedPerUpgrade;
                break;

            case "Health":
                if (playerHealth != null) playerHealth.AddMaxHealth(healthPerUpgrade);
                break;
        }
    }
}
