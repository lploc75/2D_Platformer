using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFadeToColor : MonoBehaviour
{
    public Tilemap tilemap; // <-- Đổi sang Tilemap thay vì TilemapRenderer
    public Color targetColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    public float duration = 2f;

    private Color startColor;
    private float timer = 0f;
    private bool fading = false;

    void Start()
    {
        if (tilemap == null) return;
        startColor = tilemap.color;
        fading = true;
    }

    void Update()
    {
        if (!fading) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        tilemap.color = Color.Lerp(startColor, targetColor, t);

        if (t >= 1f) fading = false;
    }
}
