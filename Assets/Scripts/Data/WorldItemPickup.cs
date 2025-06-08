using UnityEngine;

public class WorldItemPickup : MonoBehaviour
{
    public ItemData itemData; // Kéo asset vào đây
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (itemData != null)
        {
            spriteRenderer.sprite = itemData.icon; // Gán icon vũ khí/giáp tự động từ ScriptableObject
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Va chạm với Player!");

            if (InventoryManager.Instance == null)
            {
                Debug.LogError("InventoryManager.Instance đang NULL!");
            }
            else
            {
                InventoryManager.Instance.PickUpItem(itemData);
                Destroy(gameObject);
            }
        }
    }


}
