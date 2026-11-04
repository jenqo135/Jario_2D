using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int hp = 3;
    public int maxHp = 3;

    private bool invincible = false;
    private SpriteRenderer sr;

    public AudioSource audioSource;
    public AudioClip hurtClip;
    public AudioClip healClip;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        if (invincible) return;

        hp -= amount;
        UpdateHealthUI();

        if (audioSource != null && hurtClip != null) audioSource.PlayOneShot(hurtClip);

        if (hp <= 0)
        {
            GetComponent<GameOverManager>()?.RestartScene();
        }
        else
        {
            StartCoroutine(Invincibility());
        }
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
        UpdateHealthUI();

        if (audioSource != null && healClip != null) audioSource.PlayOneShot(healClip);
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