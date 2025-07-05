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

    /// <summary>
    /// Thêm item, nếu đã có thì cộng dồn số lượng, không thì add slot mới
    /// </summary>
    public void AddItem(ItemData item, int amount = 1)
    {
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

        if (uiController != null)
            uiController.UpdateInventorySlots();
        Debug.Log($"Đã nhận: {item.itemName} x{amount}");
    }

    /// <summary>
    /// Xóa 1 số lượng item khỏi slot (ví dụ khi dùng hoặc vứt)
    /// </summary>
    public void RemoveItem(ItemData item, int amount = 1)
    {
        InventorySlot slot = inventoryItems.Find(x => x.item == item);
        if (slot != null)
        {
            slot.amount -= amount;
            if (slot.amount <= 0)
                inventoryItems.Remove(slot);

            if (uiController != null)
                uiController.UpdateInventorySlots();
        }
    }
}