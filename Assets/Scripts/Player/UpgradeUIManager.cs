using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject upgradeCanvas;

    [Header("Buttons")]
    public Button leftButton;
    public TMP_Text leftTitle;
    public TMP_Text leftDesc;

    public Button rightButton;
    public TMP_Text rightTitle;
    public TMP_Text rightDesc;

    private PlayerUpgrade.UpgradeType leftChoice;
    private PlayerUpgrade.UpgradeType rightChoice;

    private PlayerUpgrade playerUpgrade;
    private SpawnManager spawnManager;
    private PlayerInput playerInput;

    private void Start()
    {
        playerUpgrade = FindAnyObjectByType<PlayerUpgrade>();
        spawnManager = FindAnyObjectByType<SpawnManager>();
        playerInput = playerUpgrade.GetComponent<PlayerInput>();

        upgradeCanvas.SetActive(false);

        leftButton.onClick.AddListener(() => OnUpgradeSelected(leftChoice));
        rightButton.onClick.AddListener(() => OnUpgradeSelected(rightChoice));
    }

    public void ShowUpgradeChoices()
    {
        upgradeCanvas.SetActive(true);

        // Unlock + show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable player controls
        playerInput.enabled = false;

        var types = (PlayerUpgrade.UpgradeType[])System.Enum.GetValues(typeof(PlayerUpgrade.UpgradeType));

        leftChoice = types[Random.Range(0, types.Length)];
        rightChoice = types[Random.Range(0, types.Length)];

        SetButton(leftChoice, leftTitle, leftDesc);
        SetButton(rightChoice, rightTitle, rightDesc);
    }

    private void SetButton(PlayerUpgrade.UpgradeType type, TMP_Text title, TMP_Text desc)
    {
        title.text = type.ToString();

        switch (type)
        {
            case PlayerUpgrade.UpgradeType.FireRate: desc.text = "Shoot faster"; break;
            case PlayerUpgrade.UpgradeType.MoveSpeed: desc.text = "Move faster"; break;
            case PlayerUpgrade.UpgradeType.Damage: desc.text = "Increase bullet damage"; break;
            case PlayerUpgrade.UpgradeType.MaxHealth: desc.text = "Increase max health"; break;
        }
    }

    public void OnUpgradeSelected(PlayerUpgrade.UpgradeType choice)
    {
        playerUpgrade.ApplyUpgrade(choice);

        // Hide + lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enable movement + shooting again
        playerInput.enabled = true;

        // Hide UI
        upgradeCanvas.SetActive(false);

        // Start wave
        spawnManager.StartNextWave();
    }
}
