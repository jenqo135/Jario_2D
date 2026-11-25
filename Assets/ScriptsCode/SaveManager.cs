using System;
using System.IO;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public int unlockedLevel = 1;
    public int totalCoins = 0;
    public float musicVolume = 0.8f; // Громкость музыки (0.0001 - 1.0)
    public float sfxVolume = 0.8f;   // Громкость звуков (0.0001 - 1.0)
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

    public void SetMusicVolume(float volume)
    {
        Data.musicVolume = Mathf.Clamp(volume, 0.0001f, 1f);
        SaveGame();
    }

    public void SetSFXVolume(float volume)
    {
        Data.sfxVolume = Mathf.Clamp(volume, 0.0001f, 1f);
        SaveGame();
    }

    public void ResetSave()
    {
        Data = new GameSaveData();
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