using UnityEngine;

public class ScrollingBackground2 : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
        offset = rend.material.mainTextureOffset;
    }

    void Update()
    {
        // Di chuyển offset dựa trên thời gian
        float x = Mathf.Repeat(Time.time * scrollSpeed, 1);
        rend.material.mainTextureOffset = new Vector2(x, offset.y);
    }
}
