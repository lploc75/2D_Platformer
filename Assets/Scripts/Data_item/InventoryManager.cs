using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemData> inventoryItems = new List<ItemData>();
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
        DontDestroyOnLoad(gameObject); // Giữ lại khi chuyển scene
    }

    public void PickUpItem(ItemData item)
    {
        if (inventoryItems.Count >= maxInventorySize)
        {
            Debug.Log("Túi đồ đầy rồi!");
            return;
        }

        inventoryItems.Add(item);

        if (item is WeaponData weapon)
        {
            uiController.weaponList.Add(weapon);
            uiController.UpdateInventorySlots();
        }

        Debug.Log("Đã nhặt: " + item.itemName);
    }
}
