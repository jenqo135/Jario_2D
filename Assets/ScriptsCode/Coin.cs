using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.AddCoin(coinValue);
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}