using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Scene & Level Assets")]
    [SerializeField] private string gameSceneName = "2D 1st";
    [SerializeField] private LevelData[] levels;

    [Header("Level Select Buttons")]
    [SerializeField] private Button[] levelButtons;

    [Header("Settings & Stats UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI coinsText;

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        int unlocked = SaveManager.Instance != null ? SaveManager.Instance.Data.unlockedLevel : 1;

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
            if (musicSlider != null)
                musicSlider.SetValueWithoutNotify(SaveManager.Instance.Data.musicVolume);

            if (sfxSlider != null)
                sfxSlider.SetValueWithoutNotify(SaveManager.Instance.Data.sfxVolume);

            if (coinsText != null)
                coinsText.text = $"Coins: {SaveManager.Instance.Data.totalCoins}";
        }
    }

    public void StartLevel(int levelIndex)
    {
        if (levels == null || levelIndex < 0 || levelIndex >= levels.Length) return;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayUIClick();

        LevelSpawner.SelectedLevel = levels[levelIndex];
        SceneManager.LoadScene(gameSceneName);
    }

    public void PlayContinue()
    {
        int unlocked = SaveManager.Instance != null ? SaveManager.Instance.Data.unlockedLevel : 1;
        int targetIndex = Mathf.Clamp(unlocked - 1, 0, levels.Length - 1);
        StartLevel(targetIndex);
    }

    // Привязать к OnValueChanged у слайдера Music
    public void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value, true);
        else if (SaveManager.Instance != null)
            SaveManager.Instance.SetMusicVolume(value);
    }

    // Привязать к OnValueChanged у слайдера SFX
    public void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(value, true);
        else if (SaveManager.Instance != null)
            SaveManager.Instance.SetSFXVolume(value);
    }

    public void OnResetSaveClicked()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayUIClick();

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ResetSave();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(SaveManager.Instance.Data.musicVolume, false);
                AudioManager.Instance.SetSFXVolume(SaveManager.Instance.Data.sfxVolume, false);
            }
            RefreshUI();
        }
    }
}