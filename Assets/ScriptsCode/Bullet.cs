using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;

    void Start()
    {

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }

        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player")) return;

        BossController boss = other.GetComponent<BossController>();
        if (boss != null)
        {
            boss.TakeDamage(1);
            Destroy(gameObject);
            return;
        }

        if (other.GetComponent<Hazard>() != null || 
            other.GetComponent<Enemy>() != null || 
            other.CompareTag("Hazard"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}