using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI shopNameText; // Hi?n th? tên c?a hàng
    public Button[] itemButtons; // Các button c?a v?t ph?m
    public TextMeshProUGUI[] itemPriceTexts; // Các TextMeshPro hi?n th? giá cho t?ng v?t ph?m
    public GameObject shopPanel; // Panel c?a c?a hàng
    public List<Item> currentShopItems; // Danh sách v?t ph?m hi?n t?i
    public List<Item> weaponShopItems; // V?t ph?m c?a c?a hàng v? khí
    public List<Item> healerShopItems;  // V?t ph?m c?a c?a hàng th?y thu?c
    public List<Item> blacksmithShopItems; // V?t ph?m c?a c?a hàng th? rèn
    public TextMeshProUGUI playerMoneyText; // Hi?n th? s? vàng c?a ng??i ch?i
    public InventoryTooltipUI tooltipUI; // Tooltip c?a v?t ph?m

    public int playerGold = 500; // Bi?n l?u tr? s? vàng c?a ng??i ch?i

    public void OpenHealerShop()
    {
        shopNameText.text = "HEALER SHOP";
        UpdateShopItems(healerShopItems);
        UpdatePlayerMoneyText();
        shopPanel.SetActive(true);
    }

    public void OpenBlacksmithShop()
    {
        shopNameText.text = "BLACKSMITH SHOP";
        UpdateShopItems(blacksmithShopItems);
        UpdatePlayerMoneyText();
        shopPanel.SetActive(true);
    }

    public void OpenWeaponShop()
    {
        shopNameText.text = "WEAPON SHOP";
        UpdateShopItems(weaponShopItems);
        UpdatePlayerMoneyText();
        shopPanel.SetActive(true);
    }

    private void UpdateShopItems(List<Item> items)
    {
        currentShopItems = items;
        UpdatePlayerMoneyText(); // C?p nh?t s? vàng sau khi m? c?a hàng
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i < items.Count)
            {
                // C?p nh?t tên và icon v?t ph?m
                Image itemImage = itemButtons[i].transform.GetChild(2).GetComponent<Image>();
                itemImage.sprite = items[i].icon;
                itemImage.color = Color.white;

                // C?p nh?t giá v?t ph?m
                if (itemPriceTexts != null && i < itemPriceTexts.Length)
                {
                    itemPriceTexts[i].text = " " + items[i].price.ToString();
                }

                // Ki?m tra s? ti?n ng??i ch?i có ?? quy?t ??nh b?t ho?c t?t nút mua
                Button itemButton = itemButtons[i].transform.GetChild(3).GetComponent<Button>();
                TextMeshProUGUI itemButtonText = itemButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                if (playerGold >= items[i].price)
                {
                    itemButton.interactable = true;  // Kích ho?t nút khi ?? ti?n

                    // ??t l?i màu s?c nút khi ?? ti?n
                    itemButton.GetComponent<Image>().color = Color.white;
                    itemButtonText.color = Color.white;
                }
                else
                {
                    itemButton.interactable = false;  // Vô hi?u hóa nút khi không ?? ti?n

                    // ??i màu nút thành xám khi không ?? ti?n
                    itemButton.GetComponent<Image>().color = Color.lightGray;
                    itemButtonText.color = Color.lightGray;
                }

                // Thêm hover tooltip
                ShopItemHover itemHover = itemButtons[i].GetComponent<ShopItemHover>();
                if (itemHover != null)
                {
                    itemHover.Setup(items[i].itemData, tooltipUI);
                }

                // Thêm s? ki?n khi nh?n nút
                int index = i;  // L?u l?i index ?? s? d?ng trong listener
                itemButtons[i].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => PurchaseItem(items[index]));
                itemButtons[i].gameObject.SetActive(true);
            }
            else
            {
                itemButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // Mua v?t ph?m và tr? vàng
    private void PurchaseItem(Item item)
    {
        if (playerGold >= item.price)  // Ki?m tra có ?? vàng không
        {
            playerGold -= item.price;  // Tr? vàng khi mua
            UpdatePlayerMoneyText();  // C?p nh?t s? vàng
            Debug.Log("Purchased: " + item.name + " | Remaining Gold: " + playerGold);

            // Thêm item vào Inventory
            InventoryManager.Instance.AddItem(item.itemData, 1);  // Thêm món ?? vào Inventory
            InventoryStaticUIController.Instance.UpdateInventorySlots();

        }
        else
        {
            Debug.Log("Not enough gold to buy this item.");
        }
    }

    // C?p nh?t giao di?n s? vàng
    private void UpdatePlayerMoneyText()
    {
        // C?p nh?t s? vàng hi?n th?
        playerMoneyText.text = playerGold.ToString();

        // Sau khi c?p nh?t s? vàng, ki?m tra l?i tr?ng thái các nút mua
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i < currentShopItems.Count)
            {
                // Ki?m tra và c?p nh?t l?i nút mua
                Button itemButton = itemButtons[i].transform.GetChild(3).GetComponent<Button>();
                TextMeshProUGUI itemButtonText = itemButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                if (playerGold >= currentShopItems[i].price)
                {
                    itemButton.interactable = true;  // Kích ho?t nút khi ?? ti?n
                    itemButton.GetComponent<Image>().color = Color.white; // ??t l?i màu nút khi ?? ti?n
                    itemButtonText.color = Color.white; // ??t l?i màu ch? khi ?? ti?n
                }
                else
                {
                    itemButton.interactable = false;  // Vô hi?u hóa nút khi không ?? ti?n
                    itemButton.GetComponent<Image>().color = Color.lightGray; // ??i màu nút thành xám
                    itemButtonText.color = Color.lightGray; // ??i màu ch? thành xám
                }
            }
        }
    }
}
