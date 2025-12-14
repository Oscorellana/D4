using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    public static WaveUI Instance;

    [SerializeField] private TMP_Text waveText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateWaveText(int waveNumber)
    {
        waveText.text = $"WAVE {waveNumber}";
    }
}
