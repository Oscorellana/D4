using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public enum UpgradeType
    {
        FireRate,
        MoveSpeed,
        Damage,
        MaxHealth
    }

    [Header("Player Stats")]
    public float fireRate = 0.5f;
    public float moveSpeed = 5f;
    public int damage = 1;
    public int maxHealth = 5;

    public void ApplyUpgrade(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.FireRate:
                fireRate *= 0.8f; // faster shooting
                break;

            case UpgradeType.MoveSpeed:
                moveSpeed += 1.5f;
                break;

            case UpgradeType.Damage:
                damage += 1;
                break;

            case UpgradeType.MaxHealth:
                maxHealth += 2;
                break;
        }

        Debug.Log($"Applied upgrade: {upgrade}");
    }
}
