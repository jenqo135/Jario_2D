using UnityEngine;

public class EnemyCharger : MonoBehaviour
{
    public enum State { Idle, WindUp, Charging, Cooldown }

    [Header("State")]
    [SerializeField] private State currentState = State.Idle;

    [Header("Data & Detection")]
    [SerializeField] private EnemyStats stats;
    [SerializeField] private LayerMask obstacleLayer;

    private Transform player;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Vector2 chargeDir;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        LockPosition(true);

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null || stats == null) return;

        if (currentState == State.Idle && CanSeePlayer())
        {
            currentState = State.WindUp;
            chargeDir = (player.position - transform.position).x > 0 ? Vector2.right : Vector2.left;
            
            if (sr) sr.color = Color.red;
            Invoke(nameof(StartCharge), stats.windUpTime);

            if (AudioManager.Instance != null)
            AudioManager.Instance.PlayEnemyWindup();
        }
    }

    bool CanSeePlayer()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > stats.sightDistance) return false;

        Vector2 dir = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, obstacleLayer);
        return hit.collider == null;
    }

    void StartCharge()
    {
        currentState = State.Charging;
        LockPosition(false);
        rb.linearVelocity = chargeDir * stats.chargeSpeed;
        Invoke(nameof(StopCharge), 0.7f);
    }

    void StopCharge()
    {
        rb.linearVelocity = Vector2.zero;
        LockPosition(true);
        currentState = State.Cooldown;
        if (sr) sr.color = Color.white;
        Invoke(nameof(ResetState), stats.cooldownTime);
    }

    void ResetState() => currentState = State.Idle;

    private void LockPosition(bool lockX)
    {
        if (rb == null) return;
        rb.constraints = lockX 
            ? RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation 
            : RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && stats != null)
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(stats.damage);
            }
        }
    }
}