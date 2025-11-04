using UnityEngine;
using UnityEngine.UI;
using System;

public class TargetDummy : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Health Bar Settings")]
    public GameObject healthBarObject;
    public Slider healthSlider;
    public Image fillImage;
    public float smoothSpeed = 5f;
    private bool healthBarActive = false;

    public event Action OnDeath; // event called when dummy dies

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarObject != null)
            healthBarObject.SetActive(false);

        if (healthSlider != null)
            healthSlider.value = 1f;
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
            OnDeath?.Invoke(); // notify SpawnManager
            Destroy(gameObject);
        }
    }
}
