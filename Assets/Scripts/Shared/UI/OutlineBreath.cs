using UnityEngine;
using UnityEngine.UI;

public class OutlineBreath : MonoBehaviour
{
    public Outline outline;
    public float breathSpeed = 2.0f;
    public float minAlpha = 0.4f;
    public float maxAlpha = 1.0f;

    private Color originalColor;

    void Start()
    {
        if (outline == null)
            outline = GetComponent<Outline>();
        if (outline != null)
            originalColor = outline.effectColor; // Lưu lại màu gốc (bao gồm cả alpha)
    }

    void Update()
    {
        if (outline != null)
        {
            float t = (Mathf.Sin(Time.time * breathSpeed) + 1) / 2f;
            float a = Mathf.Lerp(minAlpha, maxAlpha, t);

            // Giữ nguyên RGB, chỉ đổi Alpha
            Color c = originalColor;
            c.a = a;
            outline.effectColor = c;
        }
    }
}
