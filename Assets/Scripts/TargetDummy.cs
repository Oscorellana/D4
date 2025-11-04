using UnityEngine;
using UnityEngine.UI;

public class TargetDummy : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Health Bar")]
    public GameObject healthBarObject; // drag the Canvas child here
    private Slider healthSlider;
    private bool healthBarActive = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarObject != null)
        {
            healthBarObject.SetActive(false); // hide at start
            healthSlider = healthBarObject.GetComponentInChildren<Slider>();
            if (healthSlider != null)
                healthSlider.value = 1f;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Enable health bar on first hit
        if (!healthBarActive && healthBarObject != null)
        {
            healthBarObject.SetActive(true);
            healthBarActive = true;
        }

        // Update slider
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }

        // Destroy dummy if health reaches 0
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
