using UnityEngine;
using UnityEngine.UI;

public class AlphaPingPong : MonoBehaviour
{
    public Image targetImage;
    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;
    public float speed = 1f;

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        float a = Mathf.Lerp(minAlpha, maxAlpha, t);
        Color c = targetImage.color;
        c.a = a;
        targetImage.color = c;
    }
}
