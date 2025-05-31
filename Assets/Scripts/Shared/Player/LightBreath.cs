using UnityEngine;
using UnityEngine.Rendering.Universal; // Quan trọng: dùng cho Light2D

public class LightBreath : MonoBehaviour
{
    public Light2D light2D;         // Kéo Light2D vào đây (nếu để null sẽ tự lấy)
    public float minIntensity = 0.8f;   // Cường độ thấp nhất
    public float maxIntensity = 1.2f;   // Cường độ cao nhất
    public float breathSpeed = 2.0f;    // Tốc độ "thở" (cao = nhanh, thấp = chậm)

    void Start()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * breathSpeed) + 1f) / 2f; // Dao động từ 0→1
        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
