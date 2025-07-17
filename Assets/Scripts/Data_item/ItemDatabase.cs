using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public static class ItemDatabase
{
    private static List<ItemData> allItems = new List<ItemData>();

    // Thêm vật phẩm vào ItemDatabase
    public static void AddItem(ItemData item)
    {
        if (!allItems.Contains(item))
        {
            allItems.Add(item);
            Debug.Log($"Đã thêm vật phẩm {item.itemName} vào ItemDatabase.");
        }
        else
        {
            Debug.Log($"Vật phẩm {item.itemName} đã tồn tại trong ItemDatabase.");
        }
    }

    // Lấy vật phẩm từ ItemDatabase theo tên
    public static ItemData GetItemByName(string itemName)
    {
        ItemData item = allItems.Find(i => i.itemName == itemName);

        // Kiểm tra cả danh sách trang bị nếu vật phẩm không tìm thấy trong ItemDatabase
        if (item == null)
        {
            item = EquipmentManager.Instance.equippedItems.Find(i => i.itemName == itemName);
        }

        if (item == null)
        {
            Debug.LogError($"Item {itemName} không tìm thấy trong ItemDatabase và trang bị.");
        }

        return item;
    }


    // Nạp tất cả vật phẩm vào ItemDatabase
    public static void LoadAllItems(List<ItemData> items)
    {
        allItems = new List<ItemData>(items);
    }

    // Lấy danh sách tất cả vật phẩm
    public static List<ItemData> GetAllItems()
    {
        return allItems;
    }

    public static void LoadItemsFromJson(string filePath)
    {
        string json = System.IO.File.ReadAllText(filePath);
        InventoryDataModel inventoryData = JsonUtility.FromJson<InventoryDataModel>(json);

        Debug.Log("Dữ liệu tệp JSON đã được tải: " + json);

        List<ItemData> items = new List<ItemData>();

        foreach (var itemData in inventoryData.items)
        {
            ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), itemData.itemType);

            ItemData item = null;

            // Tạo vật phẩm theo loại (vũ khí hay giáp)
            if (type == ItemType.Weapon)
                item = new WeaponData();
            else if (type == ItemType.Armor)
                item = new ArmorData();  // Khởi tạo ArmorData nếu là Armor

            if (item == null)
            {
                Debug.LogWarning($"Không hỗ trợ loại item {type}");
                continue;
            }

            // Gán thông tin cho vật phẩm từ JSON
            item.itemName = itemData.itemName;
            item.itemType = type;
            item.quality = (ItemQuality)System.Enum.Parse(typeof(ItemQuality), itemData.quality);
            item.description = itemData.description;
            item.icon = Resources.Load<Sprite>(itemData.iconPath);

            // Gán thêm thông tin chi tiết cho các vật phẩm cụ thể (Weapon, Armor)
            if (item is WeaponData weapon)
            {
                weapon.baseDamage = (int)itemData.baseDamage;
                weapon.critDamage = itemData.critDamage;
                weapon.critChance = itemData.critChance;
                weapon.hp = itemData.hp;
                weapon.sp = itemData.sp;
                weapon.mp = itemData.mp;
            }

            if (item is ArmorData armor)
            {
                armor.healthBonus = itemData.healthBonus;
                armor.baseDamage = (int)itemData.baseDamage;
                armor.critDamage = itemData.critDamage;
                armor.critChance = itemData.critChance;
                armor.sp = itemData.sp;
                armor.mp = itemData.mp;
            }

            items.Add(item);
            ItemDatabase.AddItem(item);  // Đảm bảo vật phẩm được thêm vào ItemDatabase
        }

        Debug.Log("Đã nạp tất cả vật phẩm vào ItemDatabase.");
    }


}
