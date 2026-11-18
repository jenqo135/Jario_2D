using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Data")]
    public GameBalanceData balance;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int value = balance != null ? balance.coinValue : 1;

            if (UIManager.Instance != null)
            {
                UIManager.Instance.AddCoin(value);
            }
            else if (SaveManager.Instance != null)
            {
                SaveManager.Instance.AddCoins(value);
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}