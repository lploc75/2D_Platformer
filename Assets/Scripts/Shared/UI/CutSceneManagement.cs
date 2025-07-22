using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro; // Để dùng TextMeshProUGUI

public class CutsceneSkip : MonoBehaviour
{
    [Header("Cutscene Settings")]
    public VideoPlayer videoPlayer;       // Kéo VideoPlayer vào đây trong Inspector
    public string nextScene = "Village";  // Tên scene sẽ chuyển tới sau cutscene

    private bool isSkipping = false;

    [Header("Skip Text (Optional)")]
    public TextMeshProUGUI skipText;      // Kéo "Hold C to skip" vào đây
    public float minAlpha = 0.2f;         // Độ trong suốt nhỏ nhất khi nhấp nháy
    public float maxAlpha = 1f;           // Độ trong suốt lớn nhất khi nhấp nháy
    public float blinkSpeed = 2f;         // Tốc độ nhấp nháy

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void Update()
    {
        // Nhấp nháy cho dòng "Hold C to skip"
        if (skipText != null)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);
            Color c = skipText.color;
            c.a = currentAlpha;
            skipText.color = c;
        }

        // Nhấn giữ phím C để bỏ qua cutscene
        if (!isSkipping && Input.GetKey(KeyCode.C))
        {
            isSkipping = true;
            SkipCutscene();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!isSkipping)
        {
            isSkipping = true;
            SkipCutscene();
        }
    }

    void SkipCutscene()
    {
        // Ẩn text khi đã skip (nếu có)
        if (skipText != null)
            skipText.gameObject.SetActive(false);

        SceneManager.LoadScene(nextScene);
    }
}
