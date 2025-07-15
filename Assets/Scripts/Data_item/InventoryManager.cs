using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [System.Serializable]
    public class InventorySlot
    {
        public ItemData item;
        public int amount;

        public InventorySlot(ItemData item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }

    public List<InventorySlot> inventoryItems = new List<InventorySlot>();
    public int maxInventorySize = 20;

    public InventoryStaticUIController uiController;
    public List<ItemData> currencyItemDataList; // Kéo asset Coin, Gem,... vào Inspector

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
    // Thêm vật phẩm vào kho (nếu vật phẩm đã có thì tăng số lượng)
    public void AddItem(ItemData item, int amount = 1)
    {
        // Kiểm tra xem vật phẩm đã có trong kho chưa
        var existingSlot = inventoryItems.FirstOrDefault(slot => slot.item == item);

        if (existingSlot != null)
        {
            // Nếu vật phẩm đã có, tăng số lượng (stack)
            existingSlot.amount += amount;
        }
        else
        {
            // Nếu vật phẩm chưa có, thêm một slot mới với số lượng là amount
            inventoryItems.Add(new InventorySlot(item, amount));
        }

        // Cập nhật lại UI kho sau khi thêm vật phẩm
        InventoryStaticUIController.Instance.UpdateInventorySlots();
    }

    /// <summary>
    /// Xóa 1 số lượng item khỏi slot (ví dụ khi dùng hoặc vứt)
    // Xóa vật phẩm khỏi kho (giảm số lượng hoặc xóa hẳn nếu số lượng = 0)
    public void RemoveItem(ItemData item, int amount = 1)
    {
        var existingSlot = inventoryItems.FirstOrDefault(slot => slot.item == item);

        if (existingSlot != null)
        {
            existingSlot.amount -= amount;

            // Nếu số lượng bằng 0, xóa vật phẩm khỏi kho
            if (existingSlot.amount <= 0)
            {
                inventoryItems.Remove(existingSlot);
            }
        }

        // Cập nhật lại UI kho sau khi xóa vật phẩm
        InventoryStaticUIController.Instance.UpdateInventorySlots();
    }

    private CurrencyData FindCurrencyItem(CurrencyType type)
    {
        foreach (var item in currencyItemDataList)
        {
            if (item is CurrencyData currencyItem && currencyItem.currencyType == type)
                return currencyItem;
        }
        return null;
    }


    public void AddCurrency(CurrencyType type, int amount)
    {
        // Tìm asset CurrencyData đúng loại
        CurrencyData currencyItem = FindCurrencyItem(type);
        if (currencyItem == null)
        {
            Debug.LogError("Chưa có asset CurrencyData cho loại tiền: " + type);
            return;
        }
        AddItem(currencyItem, amount);
    }

    public int GetCurrencyAmount(CurrencyType type)
    {
        foreach (var slot in inventoryItems)
        {
            if (slot.item is CurrencyData currencyItem && currencyItem.currencyType == type)
                return slot.amount;
        }
        return 0;
    }


}