using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData itemData;
    public InventoryTooltipUI tooltipUI;

    public void Setup(ItemData data, InventoryTooltipUI tooltip)
    {
        itemData = data;
        tooltipUI = tooltip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null && tooltipUI != null)
        {
            // Lấy vị trí slot center (có thể thay đổi tuỳ ý)
            RectTransform slotRect = GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            slotRect.GetWorldCorners(corners);
            Vector3 slotCenter = (corners[0] + corners[2]) * 0.5f;
            Vector2 slotScreenPos = RectTransformUtility.WorldToScreenPoint(null, slotCenter);

            tooltipUI.OnSlotPointerEnter(itemData, slotScreenPos);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipUI != null)
            tooltipUI.OnSlotPointerExit();
    }
}
