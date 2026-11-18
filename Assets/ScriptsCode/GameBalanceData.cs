using UnityEngine;

[CreateAssetMenu(fileName = "GameBalanceData", menuName = "Game Data/Game Balance")]
public class GameBalanceData : ScriptableObject
{
    [Header("Items & Hazards")]
    public int coinValue = 1;
    public int hazardDamage = 1;
    public int bombDamage = 2;
    public float bombFuseTime = 2f;
    public int heartHealAmount = 1;

    [Header("Enemy Bullets")]
    public float enemyBulletSpeed = 8f;
    public int enemyBulletDamage = 1;

    [Header("Boss")]
    public int bossMaxHp = 15;
    public int bossProjectilesCount = 6;
    public float bossProjectileSpeed = 10f;
    public float bossAttackInterval = 2.5f;
}