using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private int damage = 1;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Пуля летит вперед (в направлении firePoint)
            rb.linearVelocity = transform.right * speed;
        }

        // Уничтожить пулю через 4 секунды, если она ни обо что не ударилась
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Игнорируем самого стрелка и других врагов
        if (other.CompareTag("Enemy") || other.GetComponent<EnemyShooter>() != null) 
            return;

        // 2. Если попали в Игрока — наносим урон
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }

        // 3. Если попали в стену/землю — просто уничтожаем пулю
        if (other.CompareTag("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}