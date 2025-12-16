using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;
    public PlayerHealthBar healthBar;
    public GameObject deathEffect;

    bool isDead;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar == null)
            healthBar = FindFirstObjectByType<PlayerHealthBar>();

        if (healthBar != null)
            healthBar.UpdateHealthBar(1f);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth / maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        isDead = true;
        Debug.Log("PLAYER DIED");

        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Disable player systems
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        Weapon weapon = GetComponentInChildren<Weapon>();
        if (weapon != null) weapon.enabled = false;

        // Show death menu
        DeathMenuUI.Instance.Show();
    }

    public void AddMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth / maxHealth);
    }
}
