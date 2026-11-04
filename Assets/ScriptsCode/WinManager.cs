using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public GameObject winPanel;
    public string menuSceneName = "Menu";

    void Start()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    public void ShowWinScreen()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // Метод для кнопки "Вернуться в меню"
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}