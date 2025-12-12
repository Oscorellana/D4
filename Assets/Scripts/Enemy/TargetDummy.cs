using UnityEngine;
using UnityEngine.UI;
using System;

public class TargetDummy : MonoBehaviour
{
    public int maxHealth = 3;
    int currentHealth;

    [Header("Healthbar (optional)")]
    public GameObject healthBarObject;
    public Slider healthSlider;
    public Image fillImage;
    public float smoothSpeed = 5f;

    public Action OnDeath;

    bool healthBarActive = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBarObject != null) healthBarObject.SetActive(false);
        if (healthSlider != null) healthSlider.value = 1f;
    }

    void Update()
    {
        if (healthSlider != null)
            healthSlider.value = Mathf.Lerp(healthSlider.value, (float)currentHealth / maxHealth, Time.deltaTime * smoothSpeed);
        if (fillImage != null)
            fillImage.color = Color.Lerp(Color.red, Color.green, healthSlider.value);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (!healthBarActive && healthBarObject != null)
        {
            healthBarObject.SetActive(true);
            healthBarActive = true;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
