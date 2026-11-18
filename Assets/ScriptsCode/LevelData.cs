using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnEntry
{
    public GameObject prefab;
    public Vector2 position;
}

[CreateAssetMenu(fileName = "Level_01", menuName = "Game Data/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("General Info")]
    public int levelNumber = 1;
    public string levelName = "Forest Start";
    public float timeLimit = 90f;

    [Header("Player & Gate")]
    public Vector2 playerStartPos = new Vector2(-6f, 0f);
    public Vector2 gatePosition = new Vector2(15f, 0f);

    [Header("Entities to Spawn")]
    public List<SpawnEntry> enemies = new List<SpawnEntry>();
    public List<SpawnEntry> collectibles = new List<SpawnEntry>();
}