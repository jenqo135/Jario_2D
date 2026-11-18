using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene & Level Assets")]
    [SerializeField] private string gameSceneName = "2D 1st"; // Имя вашей игровой сцены
    [SerializeField] private LevelData[] levels;              // Level_01, Level_02, Level_03

    [Header("Level Select Buttons")]
    [SerializeField] private Button[] levelButtons;           // Кнопки Level 1, Level 2, Level 3

    [Header("Settings & Stats UI (Опционально)")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI coinsText;

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        int unlocked = SaveManager.Instance != null ? SaveManager.Instance.Data.unlockedLevel : 1;

        // Включаем только открытые уровни, остальные блокируем
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (levelButtons[i] != null)
            {
                bool isUnlocked = (i + 1) <= unlocked;
                levelButtons[i].interactable = isUnlocked;
            }
        }

        if (SaveManager.Instance != null)
        {
            if (volumeSlider != null)
            {
                volumeSlider.SetValueWithoutNotify(SaveManager.Instance.Data.masterVolume);
            }

            if (coinsText != null)
            {
                coinsText.text = $"Coins: {SaveManager.Instance.Data.totalCoins}";
            }
        }
    }

    // Привязать к кнопкам уровней (индекс 0 = Level 1, индекс 1 = Level 2, индекс 2 = Level 3)
    public void StartLevel(int levelIndex)
    {
        if (levels == null || levelIndex < 0 || levelIndex >= levels.Length) return;

        LevelSpawner.SelectedLevel = levels[levelIndex];
        SceneManager.LoadScene(gameSceneName);
    }

    // Привязать к кнопке "Play" / "Continue" (загружает последний открытый уровень)
    public void PlayContinue()
    {
        int unlocked = SaveManager.Instance != null ? SaveManager.Instance.Data.unlockedLevel : 1;
        int targetIndex = Mathf.Clamp(unlocked - 1, 0, levels.Length - 1);
        StartLevel(targetIndex);
    }

    // Привязать к OnValueChanged у Slider громкости
    public void OnVolumeChanged(float value)
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SetVolume(value);
        }
    }

    // Привязать к кнопке "Reset Save" (удобно для демонстрации на защите)
    public void OnResetSaveClicked()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ResetSave();
            RefreshUI();
        }
    }
}