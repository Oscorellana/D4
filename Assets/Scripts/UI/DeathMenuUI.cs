using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuUI : MonoBehaviour
{
    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += Show;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= Show;
    }

    void Show()
    {
        gameObject.SetActive(true);

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
