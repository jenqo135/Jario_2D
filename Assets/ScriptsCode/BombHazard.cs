using UnityEngine;

public class BombHazard : Hazard
{
    void Start()
    {
        Invoke(nameof(Explode), 2f);
    }

    void Explode()
    {
        Destroy(gameObject);
    }

    protected override void OnHitPlayer(Health player)
    {
        player.TakeDamage(2);
    }
}