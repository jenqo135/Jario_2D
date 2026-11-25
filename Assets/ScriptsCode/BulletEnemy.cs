using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameBalanceData balance;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null && balance != null)
        {
            rb.linearVelocity = transform.right * balance.enemyBulletSpeed;
        }

        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Игнорируем других врагов и триггеры (монеты, зоны обзора, ворота)
        if (other.CompareTag("Enemy") || other.isTrigger) return;

        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
            Destroy(gameObject);
            return;
        }

        // Любой твёрдый объект (стена, платформа, земля) уничтожает пулю
        Destroy(gameObject);
    }
}