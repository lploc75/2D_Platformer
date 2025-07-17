using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    public string fadeSaveKey = "spritefade_unique_id"; // Gán trong Inspector cho mỗi sprite
    public float fadeDuration = 2f;
    public bool fadeOut = true;

    private SpriteRenderer sr;
    private float timer = 0f;
    private float startAlpha;
    private float endAlpha;
    private bool finished = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (GameSaveManager.Instance != null && GameSaveManager.Instance.GetSpriteFadeStatus(fadeSaveKey))
        {
            // Đã fade rồi, áp dụng alpha luôn
            SetAlpha(fadeOut ? 0f : 1f);
            finished = true;
        }
        else
        {
            startAlpha = fadeOut ? 1f : 0f;
            endAlpha = fadeOut ? 0f : 1f;
            SetAlpha(startAlpha);
        }
    }

    void Update()
    {
        if (finished) return;
        if (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            SetAlpha(newAlpha);

            if (t >= 1f)
            {
                finished = true;
                SetAlpha(endAlpha);
                // Đánh dấu đã fade xong vào save
                if (GameSaveManager.Instance != null)
                    GameSaveManager.Instance.SetSpriteFadeStatus(fadeSaveKey, true);
            }
        }
    }
    void SetAlpha(float a)
    {
        if (sr == null) return;
        Color c = sr.color;
        c.a = a;
        sr.color = c;
    }
}
