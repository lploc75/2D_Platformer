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
                ItemData oldWeapon = weaponSlotUI.GetCurrentItem();
                weaponSlotUI.SetItem(item);
                if (oldWeapon != null)
                {
                    InventoryStaticUIController.Instance.inventoryItems.Add(oldWeapon);
                    InventoryStaticUIController.Instance.UpdateInventorySlots();
                }
                break;

            case ItemType.Armor:
                ItemData oldArmor = armorSlotUI.GetCurrentItem();
                armorSlotUI.SetItem(item);
                if (oldArmor != null)
                {
                    InventoryStaticUIController.Instance.inventoryItems.Add(oldArmor);
                    InventoryStaticUIController.Instance.UpdateInventorySlots();
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

