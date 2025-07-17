using UnityEngine;

public class SpritesFadeToColor : MonoBehaviour
{
    public string colorSaveKey = "spritescolorkey_unique_id"; // Gán trong Inspector
    public SpriteRenderer[] spritesToFade;
    public Color targetColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    public float duration = 2f;

    private Color[] startColors;
    private float timer = 0f;
    private bool fading = false;

    void Start()
    {
        if (GameSaveManager.Instance != null && GameSaveManager.Instance.GetSpriteColorStatus(colorSaveKey))
        {
            // Đã đổi màu xong, áp dụng luôn màu cuối
            foreach (var s in spritesToFade)
                if (s != null) s.color = targetColor;
            fading = false;
        }
        else
        {
            if (spritesToFade.Length == 0) return;
            startColors = new Color[spritesToFade.Length];
            for (int i = 0; i < spritesToFade.Length; i++)
                startColors[i] = spritesToFade[i].color;
            fading = true;
        }
    }

    void Update()
    {
        if (!fading) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        for (int i = 0; i < spritesToFade.Length; i++)
            spritesToFade[i].color = Color.Lerp(startColors[i], targetColor, t);

        if (t >= 1f)
        {
            fading = false;
            // Đánh dấu đã đổi màu xong vào save
            if (GameSaveManager.Instance != null)
                GameSaveManager.Instance.SetSpriteColorStatus(colorSaveKey, true);
        }
    }
}
