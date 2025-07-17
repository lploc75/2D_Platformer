using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class InventorySlotDataModel
{
    public string itemName;
    public int amount;
    public string quality;
    public string iconPath;
    public string itemType;
    public string description;
    public float baseDamage;
    public float critDamage;
    public float critChance;
    public float hp;
    public float sp;
    public float mp;

    // Thêm thông tin giáp
    public float healthBonus;   // Lưu thông tin máu cộng thêm từ giáp

    public InventorySlotDataModel(InventoryManager.InventorySlot inventorySlot)
    {
        itemName = inventorySlot.item.itemName;
        amount = inventorySlot.amount;
        quality = inventorySlot.item.quality.ToString();  // Giả sử quality là enum
        iconPath = "Icons/" + inventorySlot.item.icon.name;  // Lưu tên ảnh từ Unity (không phải đối tượng Sprite)
        description = inventorySlot.item.description;
        itemType = inventorySlot.item.itemType.ToString();

        // Kiểm tra nếu vật phẩm là Weapon hoặc Armor
        if (inventorySlot.item is WeaponData weapon)
        {
            baseDamage = weapon.baseDamage;
            critDamage = weapon.critDamage;
            critChance = weapon.critChance;
            hp = weapon.hp;
            sp = weapon.sp;
            mp = weapon.mp;
        }

        // Nếu là Armor, lưu thông tin liên quan đến giáp
        if (inventorySlot.item is ArmorData armor)
        {
            healthBonus = armor.GetHealthBonus();  // Lưu máu cộng thêm từ giáp
            baseDamage = armor.GetBaseDamage();    // Lưu sát thương cơ bản từ giáp
            critDamage = armor.GetCritDamage();    // Lưu sát thương chí mạng từ giáp
            critChance = armor.GetCritChance();    // Lưu tỷ lệ chí mạng từ giáp
            sp = armor.GetSp();                   // Lưu stamina từ giáp
            mp = armor.GetMp();                   // Lưu mana từ giáp
        }
    }
}

// Định nghĩa ngoài `InventorySlotDataModel`
[System.Serializable]
public class InventoryDataModel
{
    public List<InventorySlotDataModel> items;

    public InventoryDataModel(List<InventoryManager.InventorySlot> inventoryItems)
    {
        items = inventoryItems
            .Where(item => item.item.itemType == ItemType.Weapon || item.item.itemType == ItemType.Armor)  // Lọc chỉ vũ khí và áo giáp
            .Select(item => new InventorySlotDataModel(item))  // Chuyển sang InventorySlotDataModel
            .ToList();
    }
}
