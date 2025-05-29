using UnityEngine;

public class UVScroll : MonoBehaviour
{
    public float scrollSpeedX = 0.2f; // Tùy chỉnh
    public float scrollSpeedY = 0f;

    public Transform playerTransform;  // Kéo player vào trường này

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (playerTransform == null) return;

        float x = playerTransform.position.x * scrollSpeedX;
        float y = playerTransform.position.y * scrollSpeedY;
        rend.material.mainTextureOffset = new Vector2(x, y);
    }
}
