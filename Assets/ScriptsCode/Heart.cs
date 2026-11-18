public class Heart : Hazard
{
    protected override void OnHitPlayer(Health player)
    {
        if (balance != null)
            player.Heal(balance.heartHealAmount);
    }
}