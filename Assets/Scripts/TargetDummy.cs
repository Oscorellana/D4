using UnityEngine;
using UnityEngine.UI;

public class TargetDummy : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 1;
    private int currentHealth;

    [Header("Health Bar")]
    public GameObject healthBarPrefab;
    private Slider healthSlider;
    private GameObject healthBarInstance;
    private bool healthBarActive = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Create health bar if it doesn't exist yet
        if (!healthBarActive && healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            healthSlider = healthBarInstance.GetComponentInChildren<Slider>();
            healthBarActive = true;

            // Make it follow the dummy
            HealthBarFollow followScript = healthBarInstance.AddComponent<HealthBarFollow>();
            followScript.target = transform;
        }

        // Update health slider
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }

        // Destroy dummy if health reaches 0
        if (currentHealth <= 0)
        {
            if (healthBarInstance != null)
                Destroy(healthBarInstance);

            Destroy(gameObject);
        }
    }
}
