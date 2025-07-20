using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CinematicSceneFadeInOut : MonoBehaviour
{
    [Header("UI Bars")]
    public Image topBar;
    public Image bottomBar;
    public float topBarHeight = 100f;
    public float bottomBarHeight = 100f;

    [Header("Fade Settings")]
    public Image fadeImage;         // UI Image màu đen full screen
    public float fadeInDuration = 1f;
    public float barsDuration = 3f;
    public float fadeOutDuration = 1f;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    void Start()
    {
        TriggerCinematic();
    }

    public void TriggerCinematic()
    {
        StartCoroutine(CinematicRoutine());
    }

    IEnumerator CinematicRoutine()
    {
        // Đảm bảo bar đúng chiều cao và visible
        SetupBar(topBar, topBarHeight, true);
        SetupBar(bottomBar, bottomBarHeight, false);

        // Fading in từ đen sang trong suốt
        yield return StartCoroutine(Fade(1f, 0f, fadeInDuration));

        // Đợi barsDuration giây
        yield return new WaitForSeconds(barsDuration);

        // Fade out toàn màn hình (trắng sang đen)
        yield return StartCoroutine(Fade(0f, 1f, fadeOutDuration));

        // Chuyển scene
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void SetupBar(Image bar, float height, bool isTop)
    {
        if (bar == null) return;
        var rt = bar.rectTransform;
        if (isTop)
        {
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 1);
        }
        else
        {
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 0);
            rt.pivot = new Vector2(0.5f, 0);
        }
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(0, height);
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        if (fadeImage == null)
        {
            Debug.LogError("fadeImage chưa được gán!");
            yield break;
        }
        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            color.a = Mathf.Lerp(from, to, t);
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }
        color.a = to;
        fadeImage.color = color;
    }
}
