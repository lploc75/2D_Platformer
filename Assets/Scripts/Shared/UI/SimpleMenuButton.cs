using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image imageHover;
    public Image imageSelected;

    private bool isHovering = false;
    private bool isSelected = false;

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        UpdateState();
    }

    void UpdateState()
    {
        // Selected bật khi nút được chọn
        imageSelected.enabled = isSelected;

        // Hover bật nếu:
        // - Đang hover (chưa selected)
        // - HOẶC đang selected (hover luôn bật khi selected)
        imageHover.enabled = isHovering || isSelected;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        UpdateState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        UpdateState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var menu = GetComponentInParent<MenuManager>();
        if (menu != null)
            menu.SetSelectedButton(this);
    }
}
