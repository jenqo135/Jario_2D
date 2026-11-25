using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    [Header("Настройки перехода")]
    public float delayBeforeLoad = 0.5f;

    private bool isLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            Invoke(nameof(CompleteLevel), delayBeforeLoad);
        }
    }

    private void CompleteLevel()
    {
        LevelSpawner spawner = FindFirstObjectByType<LevelSpawner>();
        if (spawner == null) return;

        // Сохраняем прогресс (разблокируем следующий уровень в save.json)
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.UnlockNextLevel(spawner.GetCurrentLevelNumber());
        }

        LevelData nextLevel = spawner.GetNextLevel();
        if (nextLevel != null)
        {
            LevelSpawner.SelectedLevel = nextLevel;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            LevelSpawner.SelectedLevel = null;
            SceneManager.LoadScene("Menu");
        }
        if (AudioManager.Instance != null) AudioManager.Instance.PlayWin();
    }
}