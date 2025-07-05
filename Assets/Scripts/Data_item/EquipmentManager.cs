using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public SlotEquipUI weaponSlotUI;
    public SlotEquipUI armorSlotUI;
    // ... thêm các loại trang bị khác nếu có

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Trang bị một món item (item phải là loại trang bị hợp lệ). 
    /// Khi trang bị, item sẽ bị remove khỏi inventory, 
    /// nếu có trang bị cũ thì add lại vào inventory (cộng stack).
    /// </summary>
    public void Equip(ItemData item)
    {
        if (item == null) return;

        switch (item.itemType)
        {
            case ItemType.Weapon:
                // Lấy item đang trang bị cũ (nếu có)
                ItemData oldWeapon = weaponSlotUI.GetCurrentItem();
                // Trang bị item mới
                weaponSlotUI.SetItem(item);
                // Nếu có trang bị cũ, add lại vào inventory (stack)
                if (oldWeapon != null)
                {
                    InventoryManager.Instance.AddItem(oldWeapon, 1);
                }
                break;

            case ItemType.Armor:
                ItemData oldArmor = armorSlotUI.GetCurrentItem();
                armorSlotUI.SetItem(item);
                if (oldArmor != null)
                {
                    InventoryManager.Instance.AddItem(oldArmor, 1);
                }
                break;

            // ... các loại khác tương tự
            default:
                Debug.LogWarning("Loại item không hợp lệ khi trang bị: " + item.itemType);
                break;
        }
        // Sau khi trang bị xong, update UI inventory
        InventoryManager.Instance.uiController.UpdateInventorySlots();
        Debug.Log($"Đã trang bị: {item.itemName} vào slot {item.itemType}");
    }

    /// <summary>
    /// Tháo trang bị khỏi slot (ví dụ khi bấm nút "unequip" trên UI)
    /// </summary>
    public void Unequip(ItemType type)
    {
        switch (type)
        {
            case ItemType.Weapon:
                ItemData weapon = weaponSlotUI.GetCurrentItem();
                if (weapon != null)
                {
                    InventoryManager.Instance.AddItem(weapon, 1);
                    weaponSlotUI.SetItem(null);
                }
                break;

            case ItemType.Armor:
                ItemData armor = armorSlotUI.GetCurrentItem();
                if (armor != null)
                {
                    InventoryManager.Instance.AddItem(armor, 1);
                    armorSlotUI.SetItem(null);
                }
                break;
                // ... các loại khác tương tự
        }
        InventoryManager.Instance.uiController.UpdateInventorySlots();
    }
}