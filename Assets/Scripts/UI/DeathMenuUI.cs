using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuUI : MonoBehaviour
{
    public static DeathMenuUI Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show()
    {
        Debug.Log("DEATH MENU SHOWN");

        gameObject.SetActive(true);

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        Debug.Log("RESTART BUTTON PRESSED");

        Time.timeScale = 1f;

        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Reloading scene: " + sceneName);

        SceneManager.LoadScene(sceneName);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
