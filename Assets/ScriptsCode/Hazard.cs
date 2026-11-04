using UnityEngine;

public class Hazard : MonoBehaviour
{
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
        player.TakeDamage(1);
    }
}