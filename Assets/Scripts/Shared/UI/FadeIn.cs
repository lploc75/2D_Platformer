using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeIn : MonoBehaviour
{
    public Image fadeImage;           // Kéo Image (UI) phủ full màn hình vào đây
    public float fadeDuration = 1f;   // Thời gian fade-in (giây)

    void Start()
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeInRoutine());
        }
    }

    System.Collections.IEnumerator FadeInRoutine()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false); // Ẩn đi khi fade xong
    }
}
