using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;

    public float smoothSpeed = 8f;
    public float hideDelay = 2f;

    Camera cam;
    float targetValue;
    float hideTimer;

    void Awake()
    {
        cam = Camera.main;
        gameObject.SetActive(false);
    }

    public void Init(int maxHealth)
    {
        gameObject.SetActive(true);

        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        targetValue = maxHealth;

        UpdateColor();
    }

    public void SetHealth(int hp)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        targetValue = hp;
        hideTimer = hideDelay;

        UpdateColor();
    }

    void Update()
    {
        // Smooth slider movement
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothSpeed);

        // Hide after delay
        if (gameObject.activeSelf)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
                gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;
        transform.forward = cam.transform.forward;
    }

    void UpdateColor()
    {
        float percent = targetValue / slider.maxValue;

        if (percent > 0.6f)
            fillImage.color = Color.green;
        else if (percent > 0.3f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;
    }
}
