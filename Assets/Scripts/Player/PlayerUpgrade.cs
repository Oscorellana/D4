using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public enum UpgradeType { ShotCooldown, Damage, Speed, Health }

    [Header("Stats")]
    public float fireRate = 0.25f; // seconds between shots (lower = faster)
    public int damage = 1;
    public float moveSpeed = 5f;
    public int maxHealth = 100;

    [Header("Optional references")]
    public Weapon weapon;
    public PlayerController playerController;
    public PlayerHealth playerHealth;
    public GameObject bulletPrefab;

    void Start()
    {
        if (weapon == null) weapon = FindFirstObjectByType<Weapon>();
        if (playerController == null) playerController = FindFirstObjectByType<PlayerController>();
        if (playerHealth == null) playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (bulletPrefab == null && weapon != null) bulletPrefab = weapon.bulletPrefab;
    }

    public void ApplyUpgrade(UpgradeType u)
    {
        Debug.Log($"PlayerUpgrade: Applying {u}");
        switch (u)
        {
            case UpgradeType.ShotCooldown:
                fireRate = Mathf.Max(0.03f, fireRate * 0.85f);
                if (weapon != null) weapon.baseFireRate = fireRate;
                break;
            case UpgradeType.Damage:
                damage += 1;
                if (bulletPrefab != null)
                {
                    Bullet b = bulletPrefab.GetComponent<Bullet>();
                    if (b != null) { /* prefab base damage not in Bullet class - damage read from PlayerUpgrade on hit */ }
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
