using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown = 0.3f;
    private float nextShot;
    private SpriteRenderer sr;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootClip;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    // вызывается из PlayerController через OnFire
    public void Shoot()
    {
        if (Time.time < nextShot) return;
        nextShot = Time.time + cooldown;

        Quaternion rotation = (sr != null && sr.flipX) ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        Instantiate(bulletPrefab, transform.position, rotation);

        if (audioSource != null && shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }
    }
}