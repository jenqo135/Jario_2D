using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer & Snapshots")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerSnapshot normalSnapshot;
    [SerializeField] private AudioMixerSnapshot pausedSnapshot;

    [Header("Music Tracks")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private float crossfadeDuration = 1.0f;

    [Header("8+ Sound Events (SFX)")]
    public AudioClip[] jumpClips;        // 1. Прыжок (можно 1-3 клипа)
    public AudioClip[] hitClips;         // 2. Урон / попадание
    public AudioClip[] pickupClips;      // 3. Подбор монеты
    public AudioClip enemyWindupClip;    // 4. Подготовка атаки врага (ДО удара!)
    public AudioClip deathClip;          // 5. Смерть игрока/врага
    public AudioClip uiClickClip;        // 6. Клик по кнопке UI
    public AudioClip winClip;            // 7. Победа / прохождение уровня
    public AudioClip loseClip;           // 8. Проигрыш

    private AudioSource musicSourceA;
    private AudioSource musicSourceB;
    private AudioSource sfxSource;
    private bool usingSourceA = true;
    private AudioClip currentTrack;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Создаём 2 источника для кроссфейда музыки и 1 для SFX
        musicSourceA = gameObject.AddComponent<AudioSource>();
        musicSourceB = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSourceA.outputAudioMixerGroup = musicGroup;
        musicSourceB.outputAudioMixerGroup = musicGroup;
        sfxSource.outputAudioMixerGroup = sfxGroup;

        musicSourceA.loop = true;
        musicSourceB.loop = true;
        musicSourceA.playOnAwake = false;
        musicSourceB.playOnAwake = false;
        sfxSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Применяем сохранённую громкость при старте игры
        if (SaveManager.Instance != null)
        {
            SetMusicVolume(SaveManager.Instance.Data.musicVolume, false);
            SetSFXVolume(SaveManager.Instance.Data.sfxVolume, false);
        }

        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Определяем, какой трек должен играть на сцене
        AudioClip targetTrack = (scene.name == "MainMenu") ? menuMusic : levelMusic;

        // Если этот трек УЖЕ играет (например, переход Level 1 -> Level 2), не перезапускаем его!
        if (targetTrack != null && targetTrack != currentTrack)
        {
            CrossfadeMusic(targetTrack);
        }

        SetPausedSnapshot(false);
    }

    // --- МУЗЫКА И КРОССФЕЙД ---

    public void CrossfadeMusic(AudioClip newClip)
    {
        if (newClip == null || newClip == currentTrack) return;
        currentTrack = newClip;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeTrackRoutine(newClip));
    }

    private IEnumerator FadeTrackRoutine(AudioClip newClip)
    {
        AudioSource active = usingSourceA ? musicSourceA : musicSourceB;
        AudioSource next = usingSourceA ? musicSourceB : musicSourceA;
        usingSourceA = !usingSourceA;

        next.clip = newClip;
        next.volume = 0f;
        next.Play();

        float startVol = active.volume;
        float timer = 0f;

        while (timer < crossfadeDuration)
        {
            timer += Time.unscaledDeltaTime; // Работает даже на паузе
            float t = timer / crossfadeDuration;
            active.volume = Mathf.Lerp(startVol, 0f, t);
            next.volume = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        active.Stop();
        active.volume = 0f;
        next.volume = 1f;
    }

    // --- ЗВУКОВЫЕ ЭФФЕКТЫ (SFX) ---

    public void PlaySFX(AudioClip clip, bool randomPitch = true)
    {
        if (clip == null) return;
        sfxSource.pitch = randomPitch ? Random.Range(0.92f, 1.08f) : 1f;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayRandomSFX(AudioClip[] clips, bool randomPitch = true)
    {
        if (clips == null || clips.Length == 0) return;
        AudioClip chosen = clips[Random.Range(0, clips.Length)];
        PlaySFX(chosen, randomPitch);
    }

    // Удобные методы для всех 8 событий
    public void PlayJump() => PlayRandomSFX(jumpClips, true);
    public void PlayHit() => PlayRandomSFX(hitClips, true);
    public void PlayPickup() => PlayRandomSFX(pickupClips, true);
    public void PlayEnemyWindup() => PlaySFX(enemyWindupClip, true);
    public void PlayDeath() => PlaySFX(deathClip, false);
    public void PlayUIClick() => PlaySFX(uiClickClip, false);
    public void PlayWin() => PlaySFX(winClip, false);
    public void PlayLose() => PlaySFX(loseClip, false);

    // --- ГРОМКОСТЬ (Log10 * 20) И ПАУЗА ---

    public void SetMusicVolume(float linearValue, bool save = true)
    {
        float clamped = Mathf.Clamp(linearValue, 0.0001f, 1f);
        float dB = Mathf.Log10(clamped) * 20f;
        if (mainMixer != null) mainMixer.SetFloat("MusicVolume", dB);

        if (save && SaveManager.Instance != null)
            SaveManager.Instance.SetMusicVolume(clamped);
    }

    public void SetSFXVolume(float linearValue, bool save = true)
    {
        float clamped = Mathf.Clamp(linearValue, 0.0001f, 1f);
        float dB = Mathf.Log10(clamped) * 20f;
        if (mainMixer != null) mainMixer.SetFloat("SFXVolume", dB);

        if (save && SaveManager.Instance != null)
            SaveManager.Instance.SetSFXVolume(clamped);
    }

    public void SetPausedSnapshot(bool isPaused)
    {
        if (pausedSnapshot != null && isPaused)
            pausedSnapshot.TransitionTo(0.25f);
        else if (normalSnapshot != null && !isPaused)
            normalSnapshot.TransitionTo(0.25f);
    }
}