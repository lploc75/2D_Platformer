using System.Collections.Generic;  // Thêm dòng này để sử dụng List<>
using System.Linq;
[System.Serializable]
public class InventorySlotDataModel
{
    public string itemName;
    public int amount;
    public string quality;
    public string iconPath;
    public string description;
    public float baseDamage;
    public float critDamage;
    public float critChance;
    public float hp;
    public float sp;
    public float mp;

    public InventorySlotDataModel(InventoryManager.InventorySlot inventorySlot)
    {
        itemName = inventorySlot.item.itemName;
        amount = inventorySlot.amount;
        quality = inventorySlot.item.quality.ToString();  // Giả sử quality là enum
        iconPath = "Icons/" + inventorySlot.item.icon.name;  // Lưu tên ảnh từ Unity (không phải đối tượng Sprite)
        description = inventorySlot.item.description;

        if (inventorySlot.item is WeaponData weapon)
        {
            baseDamage = weapon.baseDamage;
            critDamage = weapon.critDamage;
            critChance = weapon.critChance;
            hp = weapon.hp;
            sp = weapon.sp;
            mp = weapon.mp;
        }
    }
}

[System.Serializable]
public class InventoryDataModel
{
    public List<InventorySlotDataModel> items;

    public InventoryDataModel(List<InventoryManager.InventorySlot> inventoryItems)
    {
        items = inventoryItems
            .Where(item => item.item.itemType == ItemType.Weapon || item.item.itemType == ItemType.Armor)  // Lọc chỉ vũ khí và áo giáp
            .Select(item => new InventorySlotDataModel(item)) // Chuyển sang InventorySlotDataModel
            .ToList();
    }
}
