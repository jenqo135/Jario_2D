using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("Здоровье Босса")]
    public int maxHp = 15;
    private int currentHp;
    public Slider bossHpSlider;

    [Header("Настройки Атаки")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public int numberOfProjectiles = 6;
    public float projectileSpeed = 10f;
    public float attackInterval = 2.5f;

    [Header("Победа")]
    public GameObject winTrigger;

    void Start()
    {
        currentHp = maxHp;

        if (bossHpSlider != null)
        {
            bossHpSlider.maxValue = maxHp;
            bossHpSlider.value = currentHp;
            bossHpSlider.gameObject.SetActive(true);
        }

        if (winTrigger != null) winTrigger.SetActive(false);

        StartCoroutine(ShootInAllDirectionsRoutine());
    }

    IEnumerator ShootInAllDirectionsRoutine()
    {
        while (currentHp > 0)
        {
            yield return new WaitForSeconds(attackInterval);
            SpawnRadialWave();
        }
    }

    void SpawnRadialWave()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

        float angleStep = 360f / numberOfProjectiles;
        float currentAngle = 0f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float dirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            Vector2 moveDirection = new Vector2(dirX, dirY).normalized;

            GameObject proj = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = moveDirection * projectileSpeed;
            }


            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


            Destroy(proj, 4f);

            currentAngle += angleStep;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (bossHpSlider != null)
            bossHpSlider.value = currentHp;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
    if (bossHpSlider != null) bossHpSlider.gameObject.SetActive(false);


    WinManager winManager = FindObjectOfType<WinManager>();
    if (winManager != null)
    {
        winManager.ShowWinScreen();
    }

    Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }
}