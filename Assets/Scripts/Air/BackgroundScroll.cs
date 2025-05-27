using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private Renderer rend;
    private Vector2 offset;
    public Transform player;

    void Start()
    {
        rend = GetComponent<Renderer>();
        offset = rend.material.mainTextureOffset;
    }

    void Update()
    {
        float x = Mathf.Repeat(player.position.x * scrollSpeed, 1);
        rend.material.mainTextureOffset = new Vector2(x, offset.y);
    }
}
