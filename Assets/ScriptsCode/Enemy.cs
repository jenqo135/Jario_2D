using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { Patrol, AttackCooldown, Idle }
    
    [Header("State")]
    [SerializeField] private State currentState = State.Patrol;

    [Header("Data")]
    [SerializeField] private EnemyStats stats;

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
        if (stats == null) return;

        switch (currentState)
        {
            case State.Patrol:
                Move();
                break;
            case State.AttackCooldown:
                break;
            case State.Idle:
                break;
        }
    }

    void Move()
    {
        float rightBound = startPos.x + stats.patrolDistance;
        float leftBound = startPos.x - stats.patrolDistance;

        if (movingRight)
        {
            transform.Translate(Vector2.right * stats.speed * Time.deltaTime);
            if (transform.position.x >= rightBound) { movingRight = false; Flip(); }
        }
        else
        {
            transform.Translate(Vector2.left * stats.speed * Time.deltaTime);
            if (transform.position.x <= leftBound) { movingRight = true; Flip(); }
        }
    }

    void Flip()
    {
        if (sr != null) sr.flipX = !sr.flipX;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && stats != null)
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(stats.damage);
                currentState = State.AttackCooldown;
                Invoke(nameof(ResetState), stats.cooldownTime);
            }
        }
    }

    void ResetState() => currentState = State.Patrol;
}