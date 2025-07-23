using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PotionUI : MonoBehaviour
{
    public static PotionUI Instance { get; private set; }

    public TextMeshProUGUI healthPotionText;
    public TextMeshProUGUI manaPotionText;

    public List<InventoryManager.InventorySlot> inventoryItems;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ lại khi đổi scene
        }
        else
        {
            Destroy(gameObject); // tránh trùng lặp nếu có nhiều
        }
    }

    void Start()
    {
        StartCoroutine(InitPotionUI());
    }
    IEnumerator InitPotionUI()
    {
        yield return new WaitForSeconds(3f); // Delay 3 giây
        InventoryFileHandler.LoadInventoryFromFile(ref inventoryItems);
        UpdatePotionUI();
        //Debug.Log($"[PotionUI]: Đã nạp kho với {inventoryItems.Count} vật phẩm--------------------");
    }
    public void UpdatePotionUI()
    {
        var inventory = InventoryManager.Instance.inventoryItems; // ✅ Lấy từ phiên chơi (RAM)

        int healthPotionCount = GetItemCount(inventory, "Health Potion");
        int manaPotionCount = GetItemCount(inventory, "Mana Potion");

        healthPotionText.text = healthPotionCount.ToString();
        manaPotionText.text = manaPotionCount.ToString();
    }

    int GetItemCount(List<InventoryManager.InventorySlot> inventory, string itemName)
    {
        var slot = inventory.FirstOrDefault(s => s.item.itemName == itemName);
        return slot != null ? slot.amount : 0;
    }
}
