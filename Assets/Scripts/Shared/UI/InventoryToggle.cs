using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryCanvas; // Kéo Canvas Inventory vào đây trong Inspector
    public InventoryTooltipUI tooltipUI; // Kéo script InventoryTooltipUI vào đây!

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryCanvas != null)
            {
                bool isActive = inventoryCanvas.activeSelf;
                inventoryCanvas.SetActive(!isActive);

                // Khi inventory bị ẩn (tắt) thì ẩn luôn tooltip
                if (isActive && tooltipUI != null)
                {
                    tooltipUI.HideTooltipImmediate();
                }
            }
            else
            {
                Debug.LogWarning("Inventory Canvas chưa được gán!");
            }
        }
    }
}
