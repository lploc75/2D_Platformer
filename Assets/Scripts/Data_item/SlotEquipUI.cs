using UnityEngine;
using UnityEngine.UI;

public class SlotEquipUI : MonoBehaviour
{
    public Image iconImage;
    public ItemQualityUI qualityUI;
    private ItemData currentItem;
    public GameObject unequipPanel; // Popup hiện nút bỏ trang bị


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
                unequipPanel.SetActive(false); // Ẩn menu khi không còn item
        }
    }

    public ItemData GetCurrentItem()
    {
        return currentItem;
    }

    public void OnSlotClicked()
    {
        Debug.Log("Clicked slot! currentItem=" + (currentItem != null ? currentItem.itemName : "NULL"));
        if (currentItem != null && unequipPanel != null)
        {
            unequipPanel.SetActive(true);
        }
    }


    public void UnequipItem()
    {
        if (currentItem != null)
        {
            InventoryStaticUIController.Instance.inventoryItems.Add(currentItem); // Thêm lại vào kho
            InventoryStaticUIController.Instance.UpdateInventorySlots();
            SetItem(null);
            if (unequipPanel != null)
                unequipPanel.SetActive(false);
        }
    }
    public void CloseUnequipPanel()
    {
        if (unequipPanel != null)
            unequipPanel.SetActive(false);
    }



}
