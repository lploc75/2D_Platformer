using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public static class InventoryFileHandler
{
    // Phương thức để lưu kho vào tệp JSON
    public static void SaveInventoryToFile(List<InventoryManager.InventorySlot> inventoryItems)
    {
        // Chuyển danh sách vật phẩm trong kho sang mô hình dữ liệu để lưu vào JSON
        InventoryDataModel inventoryData = new InventoryDataModel(inventoryItems);

        string savePath = Path.Combine(Application.persistentDataPath, "inventory_data.json");
        string json = JsonUtility.ToJson(inventoryData, true);

        File.WriteAllText(savePath, json);
        Debug.Log("💾 Kho đã được lưu vào " + savePath);
    }



    // Phương thức để nạp vật phẩm từ tệp JSON vào ItemDatabase
    public static void LoadItemsFromJson(string filePath)
    {
        // Nạp vật phẩm từ tệp JSON vào ItemDatabase
        ItemDatabase.LoadItemsFromJson(filePath);
    }

    // Thêm phương thức LoadInventoryFromFile vào InventoryFileHandler
    public static void LoadInventoryFromFile(ref List<InventoryManager.InventorySlot> inventoryItems)
    {
        string savePath = Path.Combine(Application.persistentDataPath, "inventory_data.json");

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            InventoryDataModel inventoryData = JsonUtility.FromJson<InventoryDataModel>(json);

            // Xóa dữ liệu kho hiện tại
            inventoryItems.Clear();

            // Duyệt qua các vật phẩm trong kho đã lưu và thêm vào lại kho
            foreach (var itemData in inventoryData.items)
            {
                // Kiểm tra nếu vật phẩm đã có trong kho
                var existingSlot = inventoryItems.FirstOrDefault(slot => slot.item.itemName == itemData.itemName);
                if (existingSlot != null)
                {
                    // Nếu vật phẩm đã có, cộng dồn số lượng
                    existingSlot.amount += itemData.amount;
                }
                else
                {
                    // Nếu chưa có vật phẩm, thêm mới
                    ItemData item = ItemDatabase.GetItemByName(itemData.itemName);  // Lấy vật phẩm từ ItemDatabase
                    if (item != null)
                    {
                        inventoryItems.Add(new InventoryManager.InventorySlot(item, itemData.amount));
                    }
                    else
                    {
                        Debug.LogWarning($"Vật phẩm {itemData.itemName} không tìm thấy trong ItemDatabase.");
                    }
                }
            }

            Debug.Log("✅ Kho đã được tải thành công.");
        }
        else
        {
            Debug.LogError("⚠️ Không tìm thấy tệp kho.");
        }
    }

}

