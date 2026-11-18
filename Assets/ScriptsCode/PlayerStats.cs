using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game Data/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float speed = 7f;
    public float jumpForce = 20f;

    [Header("Health")]
    public int maxHp = 3;

    [Header("Weapon")]
    public float shootCooldown = 0.3f;
    public float bulletSpeed = 12f;
    public int bulletDamage = 1;
}