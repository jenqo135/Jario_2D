using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Требование: FSM из 3 состояний
    public enum State { Patrol, AttackCooldown, Idle }
    
    [Header("State")]
    [SerializeField] private State currentState = State.Patrol;

    [Header("Settings")]
    [SerializeField] private float speed = 2f; // Требование: SerializeField вместо public
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private int damage = 1;

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
        // Простой переключатель состояний
        switch (currentState)
        {
            case State.Patrol:
                Move();
                break;
            case State.AttackCooldown:
                // Отдыхает после удара
                break;
            case State.Idle:
                // Стоит на месте
                break;
        }
    }

    void Move()
    {
        float rightBound = startPos.x + patrolDistance;
        float leftBound = startPos.x - patrolDistance;

        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound) { movingRight = false; Flip(); }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound) { movingRight = true; Flip(); }
        }
    }

    void Flip()
    {
        if (sr != null) sr.flipX = !sr.flipX;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                currentState = State.AttackCooldown; // Переход в состояние задержки
                Invoke(nameof(ResetState), 1f); // Через 1 сек возвращаемся к патрулю
            }
        }
    }

    void ResetState() => currentState = State.Patrol;
}