using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    public SlotEquipUI weaponSlotUI;
    public SlotEquipUI armorSlotUI;
    // ...các loại slot khác nếu có

    void Awake() { Instance = this; }

    public void Equip(ItemData item)
    {
        if (item == null) return;
        switch (item.itemType)
        {
            case ItemType.Weapon:
                // Kiểm tra slot có item cũ không
                ItemData oldWeapon = weaponSlotUI.GetCurrentItem();
                weaponSlotUI.SetItem(item);
                if (oldWeapon != null)
                {
                    // Trả lại inventory!
                    InventoryStaticUIController.Instance.inventoryItems.Add(oldWeapon);
                }
                break;

            case ItemType.Armor:
                ItemData oldArmor = armorSlotUI.GetCurrentItem();
                armorSlotUI.SetItem(item);
                if (oldArmor != null)
                {
                    InventoryStaticUIController.Instance.inventoryItems.Add(oldArmor);
                }
                break;
            // ... các loại khác tương tự
            default:
                Debug.LogWarning("Loại item không hợp lệ khi trang bị: " + item.itemType);
                break;
        }
        InventoryStaticUIController.Instance.UpdateInventorySlots();
        Debug.Log($"Đã trang bị: {item.itemName} vào slot {item.itemType}");
    }
}

