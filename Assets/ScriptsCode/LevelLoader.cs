using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Настройки перехода")]
    public string nextSceneName;
    public float delayBeforeLoad = 0.5f;

    private bool isLoading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            Invoke(nameof(LoadNextScene), delayBeforeLoad);
        }
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }
}