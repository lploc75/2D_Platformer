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

        if (item == null)
        {
            Debug.LogError($"Item {itemName} không tìm thấy trong ItemDatabase.");
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
        // Đọc dữ liệu từ tệp JSON
        string json = System.IO.File.ReadAllText(filePath);
        InventoryDataModel inventoryData = JsonUtility.FromJson<InventoryDataModel>(json);

        // In ra dữ liệu tệp JSON để kiểm tra
        Debug.Log("Dữ liệu tệp JSON đã được tải: " + json);

        // Nạp vật phẩm vào ItemDatabase
        List<ItemData> items = new List<ItemData>();

        foreach (var itemData in inventoryData.items)
        {
            // Tạo ItemData từ itemData trong tệp JSON
            ItemData item = new WeaponData();  // Giả sử vật phẩm là WeaponData, bạn cần kiểm tra loại vật phẩm khác
            item.itemName = itemData.itemName;
            item.itemType = ItemType.Weapon;  // Giả sử ItemType của vật phẩm là "Weapon"
            item.quality = (ItemQuality)System.Enum.Parse(typeof(ItemQuality), itemData.quality);  // Chuyển đổi quality từ chuỗi
            item.description = itemData.description;

            // Tải icon từ đường dẫn iconPath
            item.icon = Resources.Load<Sprite>(itemData.iconPath);  // Lấy icon từ thư mục Resources

            // Cập nhật các thuộc tính của WeaponData từ tệp JSON
            if (item is WeaponData weapon)
            {
                weapon.baseDamage = (int)itemData.baseDamage; // Chuyển đổi từ float sang int
                weapon.critDamage = itemData.critDamage;
                weapon.critChance = itemData.critChance;
                weapon.hp = itemData.hp;
                weapon.sp = itemData.sp;
                weapon.mp = itemData.mp;
            }

            // Thêm vật phẩm vào danh sách
            items.Add(item);
        }

        // Nạp các vật phẩm vào ItemDatabase
        LoadAllItems(items);
        Debug.Log("Đã nạp tất cả vật phẩm vào ItemDatabase.");
    }

}
