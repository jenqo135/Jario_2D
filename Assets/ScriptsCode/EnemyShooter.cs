using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public enum State { Patrol, Aiming, Cooldown }

    [Header("State")]
    [SerializeField] private State currentState = State.Patrol;

    [Header("Data & Detection")]
    [SerializeField] private EnemyStats stats;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private Transform player;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null || stats == null) return;

        if (currentState == State.Patrol && CanSeePlayer())
        {
            currentState = State.Aiming;
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayEnemyWindup();
            if (sr) sr.color = Color.yellow;
            Invoke(nameof(Shoot), stats.windUpTime);
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

    void Shoot()
    {
        if (bulletPrefab && firePoint)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        
        currentState = State.Cooldown;
        if (sr) sr.color = Color.white;
        Invoke(nameof(ResetState), stats.cooldownTime);
    }

    void ResetState() => currentState = State.Patrol;
}