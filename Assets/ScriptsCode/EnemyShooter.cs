using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public enum State { Patrol, Aiming, Cooldown }

    [Header("State")]
    [SerializeField] private State currentState = State.Patrol;

    [Header("Raycast & Detection")]
    [SerializeField] private float sightDistance = 7f;
    [SerializeField] private LayerMask obstacleLayer; // Слой стен (чтобы Raycast не видел сквозь них)

    [Header("Attack Settings")]
    [SerializeField] private float windUpTime = 0.6f;  // Время подготовки
    [SerializeField] private float cooldownTime = 1.5f; // Время перезарядки
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
        if (player == null) return;

        if (currentState == State.Patrol && CanSeePlayer())
        {
            currentState = State.Aiming;
            if (sr) sr.color = Color.yellow; // Визуальное предупреждение (телеграфия)
            Invoke(nameof(Shoot), windUpTime); // Ждем windUpTime секунд перед выстрелом
        }
    }

    bool CanSeePlayer()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > sightDistance) return false;

        Vector2 dir = (player.position - transform.position).normalized;
        // Проверяем Raycast: если между врагом и игроком есть стена — возвращаем false
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
        Invoke(nameof(ResetState), cooldownTime);
    }

    void ResetState() => currentState = State.Patrol;
}