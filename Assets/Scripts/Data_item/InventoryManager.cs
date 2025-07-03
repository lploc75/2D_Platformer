using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [System.Serializable]
    public class InventorySlot
    {
        public ItemData item;
        public int amount;
    }

    public List<InventorySlot> inventoryItems = new List<InventorySlot>();
    public int maxInventorySize = 20;

    public InventoryStaticUIController uiController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Thêm item, cộng dồn số lượng nếu đã có
    public void AddItem(ItemData item, int amount = 1)
    {
        // Kiểm tra đã có item chưa
        InventorySlot slot = inventoryItems.Find(x => x.item == item);
        if (slot != null)
        {
            slot.amount += amount;
        }
        else
        {
            if (inventoryItems.Count >= maxInventorySize)
            {
                Debug.Log("Túi đồ đầy rồi!");
                return;
            }
            slot = new InventorySlot { item = item, amount = amount };
            inventoryItems.Add(slot);
        }

        // Nếu là vũ khí, có thể add vào weaponList
        if (item is WeaponData weapon && !uiController.weaponList.Contains(weapon))
        {
            uiController.weaponList.Add(weapon);
        }

        // Cập nhật UI
        uiController.UpdateInventorySlots();
        Debug.Log($"Đã nhận: {item.itemName} x{amount}");
    }

    // Nếu cần hàm nhặt 1 item
    public void PickUpItem(ItemData item)
    {
        AddItem(item, 1);
    }
}
