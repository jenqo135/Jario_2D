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
        if (other.CompareTag("Enemy") || other.GetComponent<EnemyShooter>() != null)
            return;

        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null && balance != null)
            {
                playerHealth.TakeDamage(balance.enemyBulletDamage);
            }
            Destroy(gameObject);
        }
    }
}