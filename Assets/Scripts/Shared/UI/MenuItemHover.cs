using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Image imageBG;
    public Image imageBorder;

    public Sprite normalSprite;
    public Sprite hoverSprite;
    public Sprite pressedSprite;
    public Sprite activeSprite;

    private bool isActive = false;

    public void SetActive(bool value)
    {
        isActive = value;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (isActive)
            imageBG.sprite = activeSprite;
        else
            imageBG.sprite = normalSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActive)
            imageBG.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActive)
            imageBG.sprite = normalSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        imageBG.sprite = pressedSprite;
        // Khi click, bạn có thể thông báo menu cha set nút này là active
    }
}
