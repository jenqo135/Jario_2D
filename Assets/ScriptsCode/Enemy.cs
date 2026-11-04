using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Движение")]
    public float speed = 2f;
    public float patrolDistance = 3f;
    public int damage = 1;

    private Vector3 startPos;
    private bool movingRight = true;
    private SpriteRenderer sr;

    void Start()
    {
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float rightBound = startPos.x + patrolDistance;
        float leftBound = startPos.x - patrolDistance;

        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        if (sr != null)
        {
            sr.flipX = !sr.flipX;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}