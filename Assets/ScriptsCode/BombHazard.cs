using UnityEngine;

public class BombHazard : Hazard
{
    void Start()
    {
        float fuseTime = balance != null ? balance.bombFuseTime : 2f;
        Invoke(nameof(Explode), fuseTime);
    }

    void Explode()
    {
        Destroy(gameObject);
    }

    protected override void OnHitPlayer(Health player)
    {
        if (balance != null)
            player.TakeDamage(balance.bombDamage);
    }
}