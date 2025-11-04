using UnityEngine;
using UnityEngine.UI;

public class TargetDummy : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Health Bar Settings")]
    public GameObject healthBarObject; // Canvas child
    public Slider healthSlider;
    public Image fillImage; // Fill Image of the slider

    public float smoothSpeed = 5f; // Speed of health bar interpolation
    private bool healthBarActive = false;
    private float targetFill;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarObject != null)
        {
            healthBarObject.SetActive(false);
        }

        if (healthSlider != null)
            healthSlider.value = 1f;

        targetFill = 1f;
    }

    void Update()
    {
        // Smoothly interpolate health bar
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetFill, Time.deltaTime * smoothSpeed);
        }

        // Smoothly update fill color
        if (fillImage != null)
        {
            fillImage.color = Color.Lerp(Color.red, Color.green, healthSlider.value);
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

        // Update target fill (used in Update for smooth animation)
        targetFill = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
