using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}