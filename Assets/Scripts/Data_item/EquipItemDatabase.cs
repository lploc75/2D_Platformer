using UnityEngine;
using System.Collections.Generic;

public static class EquipItemDatabase
{
    private static List<ItemData> equippedItems = new List<ItemData>();  // Danh sách vật phẩm đã trang bị

    // Thêm vật phẩm vào EquipItemDatabase
    public static void AddItem(ItemData item)
    {
        if (!equippedItems.Contains(item))  // Đảm bảo không thêm trùng lặp
        {
            equippedItems.Add(item);
            Debug.Log($"Đã thêm vật phẩm {item.itemName} vào Equip.");
        }
        else
        {
            Debug.Log($"Vật phẩm {item.itemName} đã tồn tại trong Equip.");
        }
    }


    // Lấy vật phẩm từ EquipItemDatabase theo tên
    public static ItemData GetItemByName(string itemName)
    {
        foreach (var item in equippedItems)
        {
            Debug.Log($"Checking item: {item.itemName} against {itemName}");
        }

        return equippedItems.Find(i => i.itemName == itemName);
    }





    // Nạp tất cả vật phẩm vào EquipItemDatabase
    public static void LoadAllItems(List<ItemData> items)
    {
        equippedItems = new List<ItemData>(items);  // Nạp lại danh sách vật phẩm trang bị
        Debug.Log("EquipItemDatabase đã được nạp.");

        // In danh sách các vật phẩm đã nạp để kiểm tra
        foreach (var item in equippedItems)
        {
            Debug.Log($"Loaded item: {item.itemName}");
        }
    }




    // Lấy danh sách tất cả vật phẩm trang bị
    public static List<ItemData> GetAllItems()
    {
        return equippedItems;
    }

    // Xóa vật phẩm khỏi EquipItemDatabase
    public static void RemoveItem(ItemData item)
    {
        if (equippedItems.Contains(item))
        {
            equippedItems.Remove(item);
            Debug.Log($"Đã xóa vật phẩm {item.itemName} khỏi Equip.");
        }
        else
        {
            Debug.LogWarning($"Vật phẩm {item.itemName} không có trong Equip.");
        }
    }
}
