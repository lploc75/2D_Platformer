using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ItemData itemData;
    InventoryTooltipUI tooltipUI;

    public void Setup(ItemData data, InventoryTooltipUI tooltip)
    {
        itemData = data;
        tooltipUI = tooltip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null && tooltipUI != null)
        {
            tooltipUI.ShowTooltip(itemData, Input.mousePosition);
            Debug.Log("Pointer enter slot: " + itemData.itemName);
        }
        else
        {
            Debug.Log("Pointer enter slot: itemData is null (slot trống)");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipUI != null)
            tooltipUI.HideTooltip();
    }
}
    