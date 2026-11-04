using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("ВЫ ПОБЕДИЛИ! Вы добрались до ворот!");
            Time.timeScale = 0f;
        }
    }
}