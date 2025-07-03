using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHoverArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryTooltipUI tooltipUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipUI.OnTooltipPointerEnter();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipUI.OnTooltipPointerExit();
    }
}
