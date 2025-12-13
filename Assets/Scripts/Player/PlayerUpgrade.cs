using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public enum UpgradeType { ShotCooldown, Damage, Speed, Health }

    public float fireRate = 0.25f;
    public int damage = 1;

    public Weapon weapon;
    public PlayerController controller;
    public PlayerHealth health;

    void Start()
    {
        weapon = FindFirstObjectByType<Weapon>();
        controller = FindFirstObjectByType<PlayerController>();
        health = FindFirstObjectByType<PlayerHealth>();
    }

    public void ApplyUpgrade(UpgradeType u)
    {
        switch (u)
        {
            case UpgradeType.ShotCooldown:
                fireRate = Mathf.Max(0.05f, fireRate * 0.8f);
                weapon.baseFireRate = fireRate;
                break;
            case UpgradeType.Damage:
                damage++;
                break;
            case UpgradeType.Speed:
                controller.walkSpeed += 1f;
                break;
            case UpgradeType.Health:
                health.AddMaxHealth(20f);
                break;
        }
    }
}
