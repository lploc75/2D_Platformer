using UnityEngine;
using UnityEngine.UI;

public class BarPulse : MonoBehaviour
{
    public Image barImage;
    public float pulseSpeed = 2.0f;
    public float minAlpha = 0.3f; // càng nhỏ càng nhấp nháy rõ
    public float maxAlpha = 1.0f;

    private Color baseColor;

    void Start()
    {
        if (barImage == null)
            barImage = GetComponent<Image>();
        baseColor = barImage.color; // màu này nên là Color.white để giữ nguyên ảnh gốc
    }

    void Update()
    {
        if (barImage != null)
        {
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2f;
            float a = Mathf.Lerp(minAlpha, maxAlpha, t);
            barImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, a);
        }
    }
}
