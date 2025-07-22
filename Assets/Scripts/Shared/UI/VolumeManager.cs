using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    [Header("Volume")]
    public Slider volumeSlider;
    public Toggle volumeToggle;
    public RectTransform volumeCheckmark;
    public float volumePosX_On = 32.285f;
    public float volumePosX_Off = -32.285f;

    [Header("Fullscreen")]
    public Toggle fullscreenToggle;
    public RectTransform fullscreenCheckmark;
    public float fullscreenPosX_On = 32.285f;
    public float fullscreenPosX_Off = -32.285f;

    [Header("Checkmark Move")]
    public float animTime = 0.12f;

    private AudioSource musicSource;
    private float lastVolume = 1f;
    private Coroutine animRoutineVolume;
    private Coroutine animRoutineFullscreen;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // SỬA: Load setting NGAY KHI VÀO GAME
        LoadSettingFromSaveFile();
    }

    void Start()
    {
        GameObject musicPlayer = GameObject.FindGameObjectWithTag("Music");
        if (musicPlayer != null)
            musicSource = musicPlayer.GetComponent<AudioSource>();

        // Đăng ký listener
        volumeSlider.onValueChanged.AddListener(OnSliderChange);
        volumeToggle.onValueChanged.AddListener(OnVolumeToggleChange);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChange);

        // Quan trọng: luôn load lại setting khi scene load lên!
        LoadSettingFromSaveFile();
    }

    // ===== LOAD FILE SETTING =====
    public void LoadSettingFromSaveFile()
    {
        string path = Path.Combine(Application.persistentDataPath, "gamesave.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<GameSaveData>(json);

            // Sync UI và Audio
            SetVolumeFromLoad(data.musicVolume);
            SetFullscreenFromLoad(data.isFullscreen);
        }
        else
        {
            SetVolumeFromLoad(1f);
            SetFullscreenFromLoad(true);
        }
    }

    public float GetCurrentVolume() => volumeSlider ? volumeSlider.value : 1f;
    public bool GetCurrentFullscreen() => fullscreenToggle ? fullscreenToggle.isOn : Screen.fullScreen;
    public void ApplyLoadedSetting(float volume, bool fullscreen)
    {
        SetVolumeFromLoad(volume);
        SetFullscreenFromLoad(fullscreen);
    }

    public void SetVolumeFromLoad(float value)
    {
        lastVolume = value > 0 ? value : 1f;
        if (volumeSlider != null) volumeSlider.value = value;
        if (volumeToggle != null) volumeToggle.isOn = value > 0;
        if (musicSource != null) musicSource.volume = value;
        SetCheckmarkPosition(volumeCheckmark, value > 0, volumePosX_On, volumePosX_Off, false);
    }

    public void SetFullscreenFromLoad(bool isFullscreen)
    {
        if (fullscreenToggle != null) fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;
        SetCheckmarkPosition(fullscreenCheckmark, isFullscreen, fullscreenPosX_On, fullscreenPosX_Off, false);
    }

    void OnSliderChange(float value)
    {
        if (value == 0)
            volumeToggle.isOn = false;
        else
        {
            if (!volumeToggle.isOn) volumeToggle.isOn = true;
            lastVolume = value;
        }
        ApplyVolume(value);
        SaveSettingToFile();
    }

    void OnVolumeToggleChange(bool isOn)
    {
        SetCheckmarkPosition(volumeCheckmark, isOn, volumePosX_On, volumePosX_Off, true);

        if (isOn)
        {
            float restoreVol = lastVolume > 0 ? lastVolume : 1f;
            if (volumeSlider.value == 0) volumeSlider.value = restoreVol;
            ApplyVolume(volumeSlider.value);
        }
        else
        {
            volumeSlider.value = 0;
            ApplyVolume(0);
        }
        SaveSettingToFile();
    }

    void ApplyVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    void OnFullscreenToggleChange(bool isOn)
    {
        SetCheckmarkPosition(fullscreenCheckmark, isOn, fullscreenPosX_On, fullscreenPosX_Off, true);

        Screen.fullScreen = isOn;
        SaveSettingToFile();
    }

    void SaveSettingToFile()
    {
        if (GameSaveManager.Instance != null)
        {
            GameSaveManager.Instance.SaveGame();
        }
        else
        {
            var data = new GameSaveData
            {
                musicVolume = GetCurrentVolume(),
                isFullscreen = GetCurrentFullscreen()
            };
            string json = JsonUtility.ToJson(data, true);
            string path = Path.Combine(Application.persistentDataPath, "gamesave.json");
            File.WriteAllText(path, json);
        }
    }

    // ... các hàm còn lại như cũ ...
    void SetCheckmarkPosition(RectTransform checkmark, bool isOn, float posX_On, float posX_Off, bool animate)
    {
        float targetX = isOn ? posX_On : posX_Off;
        if (checkmark == volumeCheckmark)
        {
            if (animate)
            {
                if (animRoutineVolume != null) StopCoroutine(animRoutineVolume);
                animRoutineVolume = StartCoroutine(MoveCheckmark(checkmark, targetX));
            }
            else
            {
                var pos = checkmark.anchoredPosition;
                pos.x = targetX;
                checkmark.anchoredPosition = pos;
            }
        }
        else
        {
            if (animate)
            {
                if (animRoutineFullscreen != null) StopCoroutine(animRoutineFullscreen);
                animRoutineFullscreen = StartCoroutine(MoveCheckmark(checkmark, targetX));
            }
            else
            {
                var pos = checkmark.anchoredPosition;
                pos.x = targetX;
                checkmark.anchoredPosition = pos;
            }
        }
    }

    System.Collections.IEnumerator MoveCheckmark(RectTransform checkmark, float targetX)
    {
        Vector2 start = checkmark.anchoredPosition;
        Vector2 end = new Vector2(targetX, start.y);
        float t = 0;
        while (t < animTime)
        {
            t += Time.deltaTime;
            checkmark.anchoredPosition = Vector2.Lerp(start, end, t / animTime);
            yield return null;
        }
        checkmark.anchoredPosition = end;
    }
}
