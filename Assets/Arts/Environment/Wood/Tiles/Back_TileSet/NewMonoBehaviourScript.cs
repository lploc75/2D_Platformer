using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float scrollSpeedX = 0.2f; // Tùy ý, thử số nhỏ trước
    public float scrollSpeedY = 0f;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float x = Time.time * scrollSpeedX;
        float y = Time.time * scrollSpeedY;
        rend.material.mainTextureOffset = new Vector2(x, y);
    }
}
