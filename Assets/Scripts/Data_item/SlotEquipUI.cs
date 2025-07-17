using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;  // Thêm vào đầu file

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

    // Lưu trạng thái trang bị vào tệp JSON
    public void SaveEquipStatus()
    {
        List<SlotEquipUIData> equipSlots = new List<SlotEquipUIData>();

        // Lưu trạng thái của vũ khí nếu có
        if (EquipmentManager.Instance.weaponSlotUI.GetCurrentItem() != null)
        {
            equipSlots.Add(new SlotEquipUIData(EquipmentManager.Instance.weaponSlotUI));  // Lưu trạng thái của weapon slot
        }

        // Lưu trạng thái của giáp nếu có
        if (EquipmentManager.Instance.armorSlotUI.GetCurrentItem() != null)
        {
            equipSlots.Add(new SlotEquipUIData(EquipmentManager.Instance.armorSlotUI));  // Lưu trạng thái của armor slot
        }

        // Lưu tất cả các trạng thái vào tệp JSON
        EquipmentSaveData saveData = new EquipmentSaveData(equipSlots);
        string savePath = Path.Combine(Application.persistentDataPath, "equip_status.json");
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Trang bị đã được lưu vào " + savePath);
    }

}
