using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;
    public PlayerHealthBar healthBar;
    public GameObject deathEffect;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar == null) healthBar = FindFirstObjectByType<PlayerHealthBar>();
        if (healthBar != null) healthBar.UpdateHealthBar(1f);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth / maxHealth);
        if (currentHealth <= 0f) Die();
    }

    void Die()
    {
        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void AddMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth / maxHealth);
    }
}
