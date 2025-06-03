using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    public float fadeDuration = 2f; // Thời gian fade (giây)
    public bool fadeOut = true;     // True: fade biến mất, False: hiện lên

    private SpriteRenderer sr;
    private float timer = 0f;
    private float startAlpha;
    private float endAlpha;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startAlpha = fadeOut ? 1f : 0f;
        endAlpha = fadeOut ? 0f : 1f;

        // Thiết lập alpha ban đầu
        Color c = sr.color;
        c.a = startAlpha;
        sr.color = c;
    }

    void Update()
    {
        if (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            Color c = sr.color;
            c.a = newAlpha;
            sr.color = c;
        }
    }
}
