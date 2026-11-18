using System;
using System.IO;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public int unlockedLevel = 1;      // 1 = открыт только 1-й уровень, 2 = 1 и 2, 3 = все
    public int totalCoins = 0;         // Общее количество монет
    public float masterVolume = 1.0f;  // Настройка громкости (0.0 - 1.0)
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public GameSaveData Data { get; private set; }
    private string saveFilePath;

    private void Awake()
    {
        // Singleton, чтобы SaveManager жил между перезагрузками уровней и меню
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        LoadGame();
    }

    public void SaveGame()
    {
        try
        {
            string json = JsonUtility.ToJson(Data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"[SaveManager] Игра сохранена в: {saveFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] Ошибка при сохранении файла: {e.Message}");
        }
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("[SaveManager] Файл сохранения не найден. Созданы стандартные настройки.");
            Data = new GameSaveData();
            SaveGame();
            return;
        }

        try
        {
            string json = File.ReadAllText(saveFilePath);
            GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);

            // Проверка на случай пустого или некорректного JSON (например, "{}" или мусор)
            if (loadedData == null || loadedData.unlockedLevel < 1)
            {
                throw new Exception("Некорректные данные внутри save.json");
            }

            Data = loadedData;
            AudioListener.volume = Data.masterVolume;
            Debug.Log($"[SaveManager] Прогресс загружен! Открыт уровень: {Data.unlockedLevel}, Монеты: {Data.totalCoins}");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[SaveManager] Файл save.json повреждён ({e.Message}). Сброс к значениям по умолчанию.");
            Data = new GameSaveData();
            SaveGame();
        }
    }

    // Вызывается при прохождении уровня
    public void UnlockNextLevel(int completedLevelNumber)
    {
        int nextLevel = completedLevelNumber + 1;
        if (nextLevel > Data.unlockedLevel)
        {
            Data.unlockedLevel = Mathf.Clamp(nextLevel, 1, 3);
        }
        SaveGame();
    }

    public void AddCoins(int amount)
    {
        Data.totalCoins += amount;
        SaveGame();
    }

    public void SetVolume(float volume)
    {
        Data.masterVolume = Mathf.Clamp01(volume);
        AudioListener.volume = Data.masterVolume;
        SaveGame();
    }

    public void ResetSave()
    {
        Data = new GameSaveData();
        AudioListener.volume = Data.masterVolume;
        SaveGame();
        Debug.Log("[SaveManager] Сохранение сброшено.");
    }

    // Автосохранение при сворачивании игры на телефоне / выходе из фокуса
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveGame();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}