using UnityEngine;
using UnityEngine.UI;

public class SlotEquipUI : MonoBehaviour
{
    public static SlotEquipUI currentSelectedSlot;

    public Image iconImage;
    public ItemQualityUI qualityUI;
    public GameObject unequipPanel; // Panel dùng chung

    private ItemData currentItem;

    public void SetItem(ItemData item)
    {
        currentItem = item;
        if (item != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
            if (qualityUI != null)
            {
                qualityUI.itemData = item;
                qualityUI.UpdateQualityFrame();
            }
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
            if (qualityUI != null)
            {
                qualityUI.itemData = null;
                qualityUI.UpdateQualityFrame();
            }
            if (unequipPanel != null)
                unequipPanel.SetActive(false);
        }
    }

    public ItemData GetCurrentItem()
    {
        return currentItem;
    }

    public void OnSlotClicked()
    {
        if (currentItem != null && unequipPanel != null)
        {
            currentSelectedSlot = this;
            unequipPanel.SetActive(true);

            RectTransform slotRect = GetComponent<RectTransform>();
            RectTransform panelRect = unequipPanel.GetComponent<RectTransform>();
            Canvas canvas = panelRect.GetComponentInParent<Canvas>();

            // Lấy screen position của slot
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                slotRect.position
            );

            // Đổi sang local point trong canvas
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out localPoint
            );

            // Đặt unequipPanel ở vị trí này (cộng offset nếu muốn)
            panelRect.anchoredPosition = localPoint + new Vector2(60, 0); // hoặc đổi offset theo ý muốn
        }
    }



    private Vector2 ScreenPointToUIPoint(Vector2 screenPoint, RectTransform panelRect)
    {
        Canvas canvas = panelRect.GetComponentInParent<Canvas>();
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPoint,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out uiPos
        );
        // Clamp (nếu cần)
        RectTransform canvasRect = canvas.transform as RectTransform;
        Vector2 panelSize = panelRect.sizeDelta;
        Vector2 canvasSize = canvasRect.sizeDelta;

        uiPos.x = Mathf.Clamp(uiPos.x, -canvasSize.x / 2, canvasSize.x / 2 - panelSize.x);
        uiPos.y = Mathf.Clamp(uiPos.y, -canvasSize.y / 2 + panelSize.y, canvasSize.y / 2);

        return uiPos;
    }



    // Được gán DUY NHẤT cho Button "Bỏ trang bị" trên Popup
    public static void OnUnequipButtonClicked()
    {
        if (currentSelectedSlot != null)
        {
            currentSelectedSlot.UnequipItem();
            currentSelectedSlot = null;
        }
    }

    // Hàm gọi logic thực sự: đưa item về inventory rồi clear slot
    public void UnequipItem()
    {
        if (currentItem != null)
        {
            // Đảm bảo sử dụng manager, không mất item
            EquipmentManager.Instance.Unequip(currentItem.itemType);

            if (unequipPanel != null)
                unequipPanel.SetActive(false);

            if (InventoryManager.Instance != null && InventoryManager.Instance.uiController != null)
                InventoryManager.Instance.uiController.UpdateInventorySlots();
        }
    }

    // Cho nút đóng popup (nếu có)
    public static void OnClosePopupButtonClicked()
    {
        if (currentSelectedSlot != null && currentSelectedSlot.unequipPanel != null)
            currentSelectedSlot.unequipPanel.SetActive(false);
        currentSelectedSlot = null;
    }
}
