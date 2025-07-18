using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public SlotEquipUI weaponSlotUI;
    public SlotEquipUI armorSlotUI;
    public List<ItemData> equippedItems = new List<ItemData>();
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
            Debug.LogWarning("Item is null when calling Equip");
            return;
        }

        Debug.Log($"Equipping item: {item.itemName}");

        ItemData oldItem = null;
        switch (item.itemType)
        {
            case ItemType.Weapon:
                oldItem = weaponSlotUI.GetCurrentItem();
                weaponSlotUI.SetItem(item);
                break;

            case ItemType.Armor:
                oldItem = armorSlotUI.GetCurrentItem();
                armorSlotUI.SetItem(item);
                break;

            default:
                Debug.LogWarning("Invalid item type for equip");
                return;
        }

        // If old item exists, return it to inventory
        if (oldItem != null)
        {
            InventoryManager.Instance.AddItem(oldItem);  // Add old item back to inventory
            EquipItemDatabase.RemoveItem(oldItem);  // Remove old item from Equip
        }

        // Remove item from inventory
        InventoryManager.Instance.RemoveItem(item);  // Remove 1 item from inventory
        EquipItemDatabase.AddItem(item);  // Add item to Equip

        Debug.Log($"Equipped item: {item.itemName} in slot {item.itemType}");

        // Update UI
        InventoryManager.Instance.uiController.UpdateInventorySlots();

        // Save equip status through the instance of SlotEquipUI
        weaponSlotUI.SaveEquipStatus();  // Save equip status for weapon
        armorSlotUI.SaveEquipStatus();   // Save equip status for armor

        // Save updated inventory
        InventoryFileHandler.SaveInventoryToFile(InventoryManager.Instance.inventoryItems);
        PlayerStatsManager.Instance.UpdateDerivedStats();
    }

    /// <summary>
    /// Tháo trang bị khỏi slot (ví dụ khi bấm nút "unequip" trên UI)
    /// </summary>
    public void Unequip(ItemType type)
    {
        ItemData item = null;

        switch (type)
        {
            case ItemType.Weapon:
                item = weaponSlotUI.GetCurrentItem();
                if (item != null)
                {
                    EquipItemDatabase.RemoveItem(item);  // Remove from Equip
                    InventoryManager.Instance.AddItem(item);  // Add back to Inventory
                    weaponSlotUI.SetItem(null);
                }
                break;

            case ItemType.Armor:
                item = armorSlotUI.GetCurrentItem();
                if (item != null)
                {
                    EquipItemDatabase.RemoveItem(item);  // Remove from Equip
                    InventoryManager.Instance.AddItem(item);  // Add back to Inventory
                    armorSlotUI.SetItem(null);
                }
                break;

            default:
                Debug.LogWarning("Invalid item type for unequip");
                break;
        }

        // Update UI and save status
        // Gọi phương thức SaveEquipStatus() từ đối tượng weaponSlotUI hoặc armorSlotUI
        if (weaponSlotUI != null)
        {
            weaponSlotUI.SaveEquipStatus();  // Lưu trạng thái của vũ khí
        }

        if (armorSlotUI != null)
        {
            armorSlotUI.SaveEquipStatus();  // Lưu trạng thái của giáp
        }

        // Cập nhật lại UI
        InventoryManager.Instance.uiController.UpdateInventorySlots();
        PlayerStatsManager.Instance.UpdateDerivedStats();
    }



    // Cập nhật phương thức LoadEquipStatus thành phương thức không tĩnh
    public void LoadEquipStatus()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "equip_status.json");

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            EquipmentSaveData equipData = JsonUtility.FromJson<EquipmentSaveData>(json);

            // Nạp các vật phẩm vào EquipItemDatabase
            List<ItemData> itemsToAdd = new List<ItemData>();
            foreach (var equipSlot in equipData.equipSlots)
            {
                ItemData item = new ItemData
                {
                    itemName = equipSlot.itemName,
                    itemType = (ItemType)Enum.Parse(typeof(ItemType), equipSlot.itemType),
                    quality = (ItemQuality)Enum.Parse(typeof(ItemQuality), equipSlot.quality),  // Lưu phẩm chất
                    description = equipSlot.description,  // Lưu mô tả vật phẩm                
                };

                // Tải Sprite từ Resources bằng iconPath
                item.icon = Resources.Load<Sprite>(equipSlot.iconPath);  // Tải ảnh từ đường dẫn "Icons/" + iconPath

                // Kiểm tra kiểu vật phẩm và chuyển đổi
                if (item.itemType == ItemType.Weapon)
                {
                    // Chuyển item thành WeaponData
                    WeaponData weapon = new WeaponData
                    {
                        itemName = item.itemName,
                        itemType = item.itemType,
                        quality = item.quality,
                        icon = item.icon,
                        description = item.description,
                        baseDamage = 0,  // Default value, update later from JSON
                        critDamage = 0,  // Default value, update later from JSON
                        critChance = 0,  // Default value, update later from JSON
                        hp = 0,          // Default value, update later from JSON
                        sp = 0,          // Default value, update later from JSON
                        mp = 0           // Default value, update later from JSON
                    };

                    // Now assign the correct values
                    weapon.baseDamage = (int)equipSlot.baseDamage;
                    weapon.critDamage = equipSlot.critDamage;
                    weapon.critChance = equipSlot.critChance;
                    weapon.hp = equipSlot.hp;
                    weapon.sp = equipSlot.sp;
                    weapon.mp = equipSlot.mp;

                    itemsToAdd.Add(weapon);
                }
                else if (item.itemType == ItemType.Armor)
                {
                    // Chuyển item thành ArmorData
                    ArmorData armor = new ArmorData
                    {
                        itemName = item.itemName,
                        itemType = item.itemType,
                        quality = item.quality,
                        icon = item.icon,
                        description = item.description,
                        baseDamage = 0,  // Default value, update later from JSON
                        critDamage = 0,  // Default value, update later from JSON
                        critChance = 0,  // Default value, update later from JSON      // Default value, update later from JSON
                        sp = 0,          // Default value, update later from JSON
                        mp = 0,          // Default value, update later from JSON
                        healthBonus = 0  // Default value, update later from JSON
                    };

                    // Now assign the correct values
                    armor.baseDamage = (int)equipSlot.baseDamage;
                    armor.critDamage = equipSlot.critDamage;
                    armor.critChance = equipSlot.critChance;
                    armor.sp = equipSlot.sp;
                    armor.mp = equipSlot.mp;
                    armor.healthBonus = equipSlot.healthBonus;

                    itemsToAdd.Add(armor);
                }
            }

            // Thêm các vật phẩm vào EquipItemDatabase
            EquipItemDatabase.LoadAllItems(itemsToAdd);

            // Kiểm tra lại danh sách trang bị đã tải
            foreach (var equipSlot in equipData.equipSlots)
            {
                // Tìm vật phẩm trong EquipItemDatabase và trang bị nó
                ItemData item = EquipItemDatabase.GetItemByName(equipSlot.itemName);

                if (item != null)
                {
                    // Cập nhật vào UI
                    if (item is WeaponData weapon)
                    {
                        Debug.Log("Cập nhật vũ khí vào slot.");
                        weaponSlotUI.SetItem(weapon);  // Cập nhật slot vũ khí
                    }
                    else if (item is ArmorData armor)
                    {
                        Debug.Log("Cập nhật giáp vào slot.");
                        armorSlotUI.SetItem(armor);  // Cập nhật slot giáp
                    }
                }
                else
                {
                    Debug.LogError($"Không tìm thấy item {equipSlot.itemName} trong EquipItemDatabase.");
                }
            }

            Debug.Log("Trang bị đã được tải từ file.");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy tệp trang bị.");
        }
    }

    public void ResetEquipment()
    {
        // Gỡ bỏ vũ khí và giáp nếu có
        Unequip(ItemType.Weapon);
        Unequip(ItemType.Armor);

        // Xóa file lưu trạng thái trang bị
        string savePath = Path.Combine(Application.persistentDataPath, "equip_status.json");
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("[EquipmentManager] Đã xóa file lưu equip_status.json.");
        }

        Debug.Log("[EquipmentManager] Đã reset toàn bộ trang bị.");
    }

}
