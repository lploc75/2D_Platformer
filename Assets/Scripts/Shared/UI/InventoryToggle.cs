using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryCanvas; // Kéo Canvas Inventory vào đây trong Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryCanvas != null)
            {
                bool isActive = inventoryCanvas.activeSelf;
                inventoryCanvas.SetActive(!isActive); // Đảo trạng thái bật/tắt
            }
            else
            {
                Debug.LogWarning("Inventory Canvas chưa được gán!");
            }
        }
    }
}
