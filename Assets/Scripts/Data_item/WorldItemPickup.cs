using UnityEngine;

public class WorldItemPickup : MonoBehaviour
{
    public ItemData itemData; // Kéo asset vào đây
    private SpriteRenderer spriteRenderer;

    [Header("Số lượng khi nhặt")]
    public int amount = 1; // Nếu muốn nhặt nhiều hơn 1 (ví dụ túi potion rơi ra 3 bình)

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (itemData != null)
        {
            spriteRenderer.sprite = itemData.icon; // Gán icon tự động
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
                // Sử dụng AddItem để cộng dồn stack đúng chuẩn
                InventoryManager.Instance.AddItem(itemData, amount);
                Destroy(gameObject);
            }
        }
    }
}