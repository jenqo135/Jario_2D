using UnityEngine;

public class EnemyCharger : MonoBehaviour
{
    public enum State { Idle, WindUp, Charging, Cooldown }

    [Header("State")]
    [SerializeField] private State currentState = State.Idle;

    [Header("Settings")]
    [SerializeField] private float sightDistance = 6f;
    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float windUpTime = 0.5f;  // Время предупреждения
    [SerializeField] private float cooldownTime = 2f;
    [SerializeField] private LayerMask obstacleLayer;

    private Transform player;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector2 chargeDir;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        if (currentState == State.Idle && CanSeePlayer())
        {
            currentState = State.WindUp;
            chargeDir = (player.position - transform.position).x > 0 ? Vector2.right : Vector2.left;
            
            if (sr) sr.color = Color.red; // Визуальное предупреждение (телеграфия)
            Invoke(nameof(StartCharge), windUpTime);
        }
    }

    bool CanSeePlayer()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > sightDistance) return false;

        Vector2 dir = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, obstacleLayer);
        return hit.collider == null;
    }

    void StartCharge()
    {
        currentState = State.Charging;
        rb.linearVelocity = chargeDir * chargeSpeed; // Рывок вперед
        Invoke(nameof(StopCharge), 0.7f); // Длительность рывка 0.7 сек
    }

    void StopCharge()
    {
        rb.linearVelocity = Vector2.zero;
        currentState = State.Cooldown;
        if (sr) sr.color = Color.white;
        Invoke(nameof(ResetState), cooldownTime);
    }

    void ResetState() => currentState = State.Idle;
    [Header("Damage")]
    [SerializeField] private int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Наносим урон игроку только во время рывка или в любом состоянии
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}