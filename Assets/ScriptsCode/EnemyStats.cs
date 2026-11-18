using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Game Data/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Movement & Patrol")]
    public float speed = 2f;
    public float patrolDistance = 3f;

    [Header("Combat & Vision")]
    public int damage = 1;
    public float sightDistance = 7f;
    public float windUpTime = 0.6f;
    public float cooldownTime = 1.5f;
    public float chargeSpeed = 12f;
}