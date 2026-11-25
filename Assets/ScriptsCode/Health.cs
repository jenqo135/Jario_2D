using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerStats stats;
    [HideInInspector] public int hp;

    private bool invincible = false;
    private bool isDead = false;
    private SpriteRenderer sr;

    public AudioSource audioSource;
    public AudioClip hurtClip;
    public AudioClip healClip;

    void Start()
    {
        if (stats != null) hp = stats.maxHp;
        sr = GetComponent<SpriteRenderer>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        if (invincible || isDead) return;

        hp -= amount;
        UpdateHealthUI();

        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            Die();
        }
        else
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayHit();
            StartCoroutine(Invincibility());
        }
    }

    private void Die()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDeath();
        }

        StartCoroutine(RestartLevelRoutine());
    }

    private IEnumerator RestartLevelRoutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        GameOverManager gom = FindFirstObjectByType<GameOverManager>();
        if (gom != null)
        {
            gom.RestartScene();
        }
        else
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        hp = Mathf.Min(hp + amount, stats.maxHp);
        UpdateHealthUI();

        if (AudioManager.Instance != null && healClip != null)
            AudioManager.Instance.PlaySFX(healClip);
    }

    void UpdateHealthUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHearts(hp);
        }
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < 10; i++)
        {
            if (sr != null) sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        if (sr != null) sr.enabled = true;
        invincible = false;
    }
}