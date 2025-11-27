using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    [Header("Player Stats (modifiable by upgrades)")]
    public float shotCooldown = 0.25f;  // seconds between shots (smaller = faster)
    public int damage = 1;
    public float moveSpeed = 5f;
    public float maxHealth = 100f;

    [Header("References (auto-find if null)")]
    public Weapon weapon;
    public PlayerController playerController;
    public PlayerHealth playerHealth;
    public GameObject bulletPrefab; // the player's bullet prefab (for damage changes)

    public enum UpgradeType { ShotCooldown, Damage, Speed, Health }

    void Start()
    {
        if (weapon == null) weapon = GetComponentInChildren<Weapon>();
        if (playerController == null) playerController = GetComponent<PlayerController>();
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
        if (bulletPrefab == null && weapon != null) bulletPrefab = weapon.bulletPrefab;
    }

    public void ApplyUpgrade(UpgradeType u)
    {
        Debug.Log("PlayerUpgrade: Applying " + u.ToString());

        switch (u)
        {
            case UpgradeType.ShotCooldown:
                shotCooldown = Mathf.Max(0.05f, shotCooldown - 0.05f);
                if (weapon != null) weapon.baseFireRate = shotCooldown;
                break;

            case UpgradeType.Damage:
                damage += 1;
                // update bullet prefab so future bullets carry increased damage
                if (bulletPrefab != null)
                {
                    Bullet b = bulletPrefab.GetComponent<Bullet>();
                    if (b != null) b.damage += 1;
                }
                break;

            case UpgradeType.Speed:
                if (playerController != null) playerController.walkSpeed += 1f;
                break;

            case UpgradeType.Health:
                if (playerHealth != null) playerHealth.AddMaxHealth(20f);
                break;
        }
    }
}
