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
        }
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
