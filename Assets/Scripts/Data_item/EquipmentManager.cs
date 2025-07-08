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
        if (item == null)
        {
            Debug.LogWarning("Item null khi gọi Equip");
            return;
        }

        Debug.Log("Đang trang bị item: " + item.itemName);
        Debug.Log("Item type: " + item.itemType);

        switch (item.itemType)
        {
            case ItemType.Weapon:
                if (weaponSlotUI == null)
                {
                    Debug.LogError("weaponSlotUI là null!");
                    return;
                }

                Debug.Log("Lấy vũ khí cũ...");
                ItemData oldWeapon = weaponSlotUI.GetCurrentItem();
                weaponSlotUI.SetItem(item);
                if (oldWeapon != null)
                {
                    Debug.Log("Thêm vũ khí cũ lại vào inventory: " + oldWeapon.itemName);
                    InventoryManager.Instance.AddItem(oldWeapon, 1);
                }
                break;

            case ItemType.Armor:
                if (armorSlotUI == null)
                {
                    Debug.LogError("armorSlotUI là null!");
                    return;
                }

                Debug.Log("Lấy giáp cũ...");
                ItemData oldArmor = armorSlotUI.GetCurrentItem();
                armorSlotUI.SetItem(item);
                if (oldArmor != null)
                {
                    Debug.Log("Thêm giáp cũ lại vào inventory: " + oldArmor.itemName);
                    InventoryManager.Instance.AddItem(oldArmor, 1);
                }
                break;

            default:
                Debug.LogWarning("Loại item không hợp lệ khi trang bị: " + item.itemType);
                break;
        }

        if (InventoryManager.Instance == null || InventoryManager.Instance.uiController == null)
        {
            Debug.LogError("InventoryManager hoặc uiController là null!");
            return;
        }

        InventoryManager.Instance.uiController.UpdateInventorySlots();
        Debug.Log($"Đã trang bị: {item.itemName} vào slot {item.itemType}");
        PlayerStatsManager.Instance.UpdateDerivedStats();

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
        PlayerStatsManager.Instance.UpdateDerivedStats();

    }
}