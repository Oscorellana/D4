using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;
    public float smoothSpeed = 5f;
    public Color fullHealthColor = Color.green;
    public Color midHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    float targetValue = 1f;

    void Start() { if (slider != null) slider.value = 1f; }

    public void UpdateHealthBar(float normalizedValue) { targetValue = Mathf.Clamp01(normalizedValue); }

    void Update()
    {
        if (slider == null || fillImage == null) return;
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothSpeed);
        Color newColor = (targetValue > 0.5f)
            ? Color.Lerp(midHealthColor, fullHealthColor, (targetValue - 0.5f) * 2f)
            : Color.Lerp(lowHealthColor, midHealthColor, targetValue * 2f);
        fillImage.color = Color.Lerp(fillImage.color, newColor, Time.deltaTime * smoothSpeed);
    }
}
