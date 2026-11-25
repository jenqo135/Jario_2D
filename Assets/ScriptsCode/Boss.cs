using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("Data")]
    public GameBalanceData balance;

    [Header("Здоровье Босса")]
    private int currentHp;
    public Slider bossHpSlider;

    [Header("Настройки Атаки")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Победа")]
    public GameObject winTrigger;

    void Start()
    {
        if (balance == null) return;

        currentHp = balance.bossMaxHp;

        if (bossHpSlider != null)
        {
            bossHpSlider.maxValue = balance.bossMaxHp;
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
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayEnemyWindup();
            yield return new WaitForSeconds(balance.bossAttackInterval);
            SpawnRadialWave();
        }
    }

    void SpawnRadialWave()
    {
        if (projectilePrefab == null || balance == null) return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;

        float angleStep = 360f / balance.bossProjectilesCount;
        float currentAngle = 0f;

        for (int i = 0; i < balance.bossProjectilesCount; i++)
        {
            float dirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            Vector2 moveDirection = new Vector2(dirX, dirY).normalized;

            GameObject proj = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = moveDirection * balance.bossProjectileSpeed;
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
            if (bossHpSlider != null) bossHpSlider.gameObject.SetActive(false);
            if (winTrigger != null) winTrigger.SetActive(true);
            Destroy(gameObject);
        }
    }
}