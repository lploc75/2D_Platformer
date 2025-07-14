using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData itemData; // D? li?u v?t ph?m liên k?t v?i nút
    public InventoryTooltipUI tooltipUI; // Tham chi?u ??n Tooltip UI

    public void Setup(ItemData data, InventoryTooltipUI tooltip)
    {
        itemData = data;
        tooltipUI = tooltip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null && tooltipUI != null)
        {
            // L?y v? trí c?a slot và hi?n th? tooltip
            RectTransform slotRect = GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            slotRect.GetWorldCorners(corners);
            Vector3 slotCenter = (corners[0] + corners[2]) * 0.5f;
            Vector2 slotScreenPos = RectTransformUtility.WorldToScreenPoint(null, slotCenter);
            slotScreenPos.x -= 100;

            tooltipUI.ShowTooltip(itemData, slotScreenPos);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipUI != null)
            tooltipUI.HideTooltipImmediate();
    }
}
