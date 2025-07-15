using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI shopNameText; // Hi?n th? t�n c?a h�ng
    public Button[] itemButtons; // C�c button c?a v?t ph?m
    public TextMeshProUGUI[] itemPriceTexts; // C�c TextMeshPro hi?n th? gi� cho t?ng v?t ph?m
    public GameObject shopPanel; // Panel c?a c?a h�ng
    public List<Item> currentShopItems; // Danh s�ch v?t ph?m hi?n t?i
    public List<Item> weaponShopItems; // V?t ph?m c?a c?a h�ng v? kh�
    public List<Item> healerShopItems;  // V?t ph?m c?a c?a h�ng th?y thu?c
    public List<Item> blacksmithShopItems; // V?t ph?m c?a c?a h�ng th? r�n
    public TextMeshProUGUI playerMoneyText; // Hi?n th? s? v�ng c?a ng??i ch?i
    public InventoryTooltipUI tooltipUI; // Tooltip c?a v?t ph?m

    public int playerGold = 500; // Bi?n l?u tr? s? v�ng c?a ng??i ch?i

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
        UpdatePlayerMoneyText(); // C?p nh?t s? v�ng sau khi m? c?a h�ng
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i < items.Count)
            {
                // C?p nh?t t�n v� icon v?t ph?m
                Image itemImage = itemButtons[i].transform.GetChild(2).GetComponent<Image>();
                itemImage.sprite = items[i].icon;
                itemImage.color = Color.white;

                // C?p nh?t gi� v?t ph?m
                if (itemPriceTexts != null && i < itemPriceTexts.Length)
                {
                    itemPriceTexts[i].text = " " + items[i].price.ToString();
                }

                // Ki?m tra s? ti?n ng??i ch?i c� ?? quy?t ??nh b?t ho?c t?t n�t mua
                Button itemButton = itemButtons[i].transform.GetChild(3).GetComponent<Button>();
                TextMeshProUGUI itemButtonText = itemButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                if (playerGold >= items[i].price)
                {
                    itemButton.interactable = true;  // K�ch ho?t n�t khi ?? ti?n

                    // ??t l?i m�u s?c n�t khi ?? ti?n
                    itemButton.GetComponent<Image>().color = Color.white;
                    itemButtonText.color = Color.white;
                }
                else
                {
                    itemButton.interactable = false;  // V� hi?u h�a n�t khi kh�ng ?? ti?n

                    // ??i m�u n�t th�nh x�m khi kh�ng ?? ti?n
                    itemButton.GetComponent<Image>().color = Color.lightGray;
                    itemButtonText.color = Color.lightGray;
                }

                // Th�m hover tooltip
                ShopItemHover itemHover = itemButtons[i].GetComponent<ShopItemHover>();
                if (itemHover != null)
                {
                    itemHover.Setup(items[i].itemData, tooltipUI);
                }

                // Th�m s? ki?n khi nh?n n�t
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

    // Mua v?t ph?m v� tr? v�ng
    private void PurchaseItem(Item item)
    {
        if (playerGold >= item.price)  // Ki?m tra c� ?? v�ng kh�ng
        {
            playerGold -= item.price;  // Tr? v�ng khi mua
            UpdatePlayerMoneyText();  // C?p nh?t s? v�ng
            Debug.Log("Purchased: " + item.name + " | Remaining Gold: " + playerGold);

            // Th�m item v�o Inventory
            InventoryManager.Instance.AddItem(item.itemData, 1);  // Th�m m�n ?? v�o Inventory
            InventoryStaticUIController.Instance.UpdateInventorySlots();

        }
        else
        {
            Debug.Log("Not enough gold to buy this item.");
        }
    }

    // C?p nh?t giao di?n s? v�ng
    private void UpdatePlayerMoneyText()
    {
        // C?p nh?t s? v�ng hi?n th?
        playerMoneyText.text = playerGold.ToString();

        // Sau khi c?p nh?t s? v�ng, ki?m tra l?i tr?ng th�i c�c n�t mua
        for (int i = 0; i < itemButtons.Length; i++)
        {
            if (i < currentShopItems.Count)
            {
                // Ki?m tra v� c?p nh?t l?i n�t mua
                Button itemButton = itemButtons[i].transform.GetChild(3).GetComponent<Button>();
                TextMeshProUGUI itemButtonText = itemButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                if (playerGold >= currentShopItems[i].price)
                {
                    itemButton.interactable = true;  // K�ch ho?t n�t khi ?? ti?n
                    itemButton.GetComponent<Image>().color = Color.white; // ??t l?i m�u n�t khi ?? ti?n
                    itemButtonText.color = Color.white; // ??t l?i m�u ch? khi ?? ti?n
                }
                else
                {
                    itemButton.interactable = false;  // V� hi?u h�a n�t khi kh�ng ?? ti?n
                    itemButton.GetComponent<Image>().color = Color.lightGray; // ??i m�u n�t th�nh x�m
                    itemButtonText.color = Color.lightGray; // ??i m�u ch? th�nh x�m
                }
            }
        }
    }
}
