using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider volumeSlider;
    private AudioSource musicSource;

    void Start()
    {
        // Tìm nhạc nền đang phát (phải gán tag hoặc tên cụ thể)
        GameObject musicPlayer = GameObject.FindGameObjectWithTag("Music");
        if (musicPlayer != null)
        {
            musicSource = musicPlayer.GetComponent<AudioSource>();
        }

        // Gán sự kiện khi kéo slider
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Load âm lượng đã lưu (nếu có)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    public void SetVolume(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
        }

        // Lưu lại âm lượng
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
