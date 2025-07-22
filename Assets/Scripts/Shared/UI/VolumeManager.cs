using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
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

    void OnEnable()
    {
        LoadSettingFromPlayerPrefs();
    }

    void Start()
    {
        GameObject musicPlayer = GameObject.FindGameObjectWithTag("Music");
        if (musicPlayer != null)
            musicSource = musicPlayer.GetComponent<AudioSource>();

        volumeSlider.onValueChanged.AddListener(OnSliderChange);
        volumeToggle.onValueChanged.AddListener(OnVolumeToggleChange);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChange);
    }

    public void LoadSettingFromPlayerPrefs()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        lastVolume = savedVolume > 0 ? savedVolume : 1f;
        volumeSlider.value = savedVolume;
        volumeToggle.isOn = savedVolume > 0;
        SetCheckmarkPosition(volumeCheckmark, volumeToggle.isOn, volumePosX_On, volumePosX_Off, false);

        bool isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;
        SetCheckmarkPosition(fullscreenCheckmark, fullscreenToggle.isOn, fullscreenPosX_On, fullscreenPosX_Off, false);

        if (musicSource != null)
            musicSource.volume = savedVolume;
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
        SaveVolume(value);
    }

    void OnVolumeToggleChange(bool isOn)
    {
        SetCheckmarkPosition(volumeCheckmark, isOn, volumePosX_On, volumePosX_Off, true);

        if (isOn)
        {
            float restoreVol = lastVolume > 0 ? lastVolume : 1f;
            if (volumeSlider.value == 0) volumeSlider.value = restoreVol;
            SaveVolume(volumeSlider.value);
        }
        else
        {
            volumeSlider.value = 0;
            SaveVolume(0);
        }
    }

    void SaveVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    void OnFullscreenToggleChange(bool isOn)
    {
        SetCheckmarkPosition(fullscreenCheckmark, isOn, fullscreenPosX_On, fullscreenPosX_Off, true);

        Screen.fullScreen = isOn;
        PlayerPrefs.SetInt("IsFullscreen", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

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
        else // fullscreenCheckmark
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
