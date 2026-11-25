using UnityEngine;
using TMPro;

public class LevelSpawner : MonoBehaviour
{
    public static LevelData SelectedLevel;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticData()
    {
        SelectedLevel = null;
    }

    [Header("Levels Pool")]
    [SerializeField] private LevelData[] allLevels; // Level_01, Level_02, Level_03

    [Header("Scene References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform gateObject; // Перетащите сюда ваши ворота со сцены!
    [SerializeField] private TextMeshProUGUI timerText;

    private LevelData currentLevel;
    private float remainingTime;
    private bool gateOpened = false;

    void Start()
    {
        currentLevel = SelectedLevel != null ? SelectedLevel : (allLevels.Length > 0 ? allLevels[0] : null);

        if (currentLevel == null)
        {
            Debug.LogError("LevelData не назначен!");
            return;
        }

        BuildLevel(currentLevel);
    }

    void BuildLevel(LevelData data)
    {
        remainingTime = data.timeLimit;

        // 1. Спавним платформы и стены этого уровня
        if (data.levelMapPrefab != null)
            Instantiate(data.levelMapPrefab, Vector3.zero, Quaternion.identity);

        // 2. Ставим игрока и ворота
        if (player != null)
            player.position = data.playerStartPos;

        if (gateObject != null)
            gateObject.position = data.gatePosition;

        // 3. Спавним врагов
        foreach (var enemy in data.enemies)
        {
            if (enemy.prefab != null)
                Instantiate(enemy.prefab, enemy.position, Quaternion.identity);
        }

        // 4. Спавним монеты и предметы
        foreach (var item in data.collectibles)
        {
            if (item.prefab != null)
                Instantiate(item.prefab, item.position, Quaternion.identity);
        }
    }

    void Update()
    {
        if (gateOpened || currentLevel == null) return;

        remainingTime -= Time.deltaTime;
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(Mathf.Max(0, remainingTime)) + "s";

        if (remainingTime <= 0f)
        {
            gateOpened = true;
            if (gateObject != null)
            {
                GateController gate = gateObject.GetComponent<GateController>();
                if (gate != null) gate.OpenGate();
            }
        }
    }

    public int GetCurrentLevelNumber() => currentLevel != null ? currentLevel.levelNumber : 1;

    public LevelData GetNextLevel()
    {
        if (currentLevel == null) return null;
        int nextIndex = currentLevel.levelNumber;
        return nextIndex < allLevels.Length ? allLevels[nextIndex] : null;
    }
}