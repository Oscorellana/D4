using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("References")]
    public Slider slider;
    public Image fillImage; // drag the Fill image

    [Header("Smoothing")]
    public float smoothSpeed = 5f;

    [Header("Colors")]
    public Color fullHealthColor = Color.green;
    public Color midHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    private float targetValue = 1f;

    void Start()
    {
        if (slider != null) slider.value = 1f;
    }

    public void UpdateHealthBar(float normalizedValue)
    {
        targetValue = Mathf.Clamp01(normalizedValue);
    }

    void Update()
    {
        if (slider == null || fillImage == null) return;
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * smoothSpeed);

        Color newColor;
        if (targetValue > 0.5f)
            newColor = Color.Lerp(midHealthColor, fullHealthColor, (targetValue - 0.5f) * 2f);
        else
            newColor = Color.Lerp(lowHealthColor, midHealthColor, targetValue * 2f);

        fillImage.color = Color.Lerp(fillImage.color, newColor, Time.deltaTime * smoothSpeed);
    }
}
