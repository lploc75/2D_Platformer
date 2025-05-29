using UnityEngine;
public class ShadowFade : MonoBehaviour
{
    public float fadeTime = 0.3f;
    private SpriteRenderer sr;
    private Color startColor;
    private float timer = 0f;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;
    }
    void Update()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(startColor.a, 0, timer / fadeTime);
        sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
        if (timer >= fadeTime)
            Destroy(gameObject);
    }
}
