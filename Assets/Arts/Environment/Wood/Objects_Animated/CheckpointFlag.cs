using UnityEngine;

public class CheckpointFlag : MonoBehaviour
{
    public Sprite unactivatedSprite; // Sprite c? ch?a k�ch ho?t (v� d? Flag_1_5)
    public Sprite activatedSprite;   // Sprite c? ?� k�ch ho?t (v� d? Flag_1_0)
    private bool isActivated = false;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = unactivatedSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            sr.sprite = activatedSprite;
            // G?i v? tr� checkpoint v? GameManager
            GameManager.Instance.SetCheckpoint(transform.position);
        }
    }
}
