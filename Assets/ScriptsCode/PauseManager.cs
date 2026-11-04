using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI Панель Паузы")]
    public GameObject pausePanel;

    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}