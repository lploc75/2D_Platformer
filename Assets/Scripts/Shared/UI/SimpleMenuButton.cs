using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image imageHover;
    public Image imageSelected;
    public GameObject contentPanel; // Gắn panel nội dung tương ứng vào đây

    private bool isHovering = false;
    private bool isSelected = false;

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        UpdateState();

        if (contentPanel != null)
            contentPanel.SetActive(selected); // Chỉ bật nếu được chọn
    }

    void UpdateState()
    {
        imageSelected.enabled = isSelected;
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
