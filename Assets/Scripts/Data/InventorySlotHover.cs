using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string description;
    InventoryTooltipUI tooltipUI;

    public void Setup(string desc, InventoryTooltipUI tooltip)
    {
        description = desc;
        tooltipUI = tooltip;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter: " + gameObject.name);
        if (!string.IsNullOrEmpty(description) && tooltipUI != null)
            tooltipUI.ShowTooltip(description, Input.mousePosition);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit: " + gameObject.name);
        if (tooltipUI != null)
            tooltipUI.HideTooltip();
    }

}
