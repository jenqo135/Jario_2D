using UnityEngine;

public class Hazard : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] protected GameBalanceData balance;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                OnHitPlayer(health);
            }
            Destroy(gameObject);
        }
    }

    protected virtual void OnHitPlayer(Health player)
    {
        if (balance != null)
            player.TakeDamage(balance.hazardDamage);
    }
}