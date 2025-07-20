using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [Header("Currency Reward")]
    [Tooltip("Các loại tiền sẽ cộng khi mở rương (cùng một lượng).")]
    public CurrencyType[] currencies = { CurrencyType.Coin };
    public Animator animator;
    public List<ItemData> rewardItems;
    public List<int> rewardAmounts;
    public RewardPanelUI rewardPanel;

    public bool isOpened = false;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rewardPanel == null)
            rewardPanel = FindFirstObjectByType<RewardPanelUI>();
    }

    private void Start()
    {
        if (isOpened)
        {
            animator.Play("Opened");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && other.CompareTag("Player"))
        {
            OpenChest();
        }
    }

    public void OpenChest()
    {
        if (isOpened) return;
        isOpened = true;

        animator.SetTrigger("Open");

        ShowRewardPanel();
    }

    public void ShowRewardPanel()
    {
        List<(ItemData, int)> rewards = new List<(ItemData, int)>();

        // Add items from the rewardItems list and their respective amounts
        for (int i = 0; i < rewardItems.Count; i++)
        {
            rewards.Add((rewardItems[i], rewardAmounts[i]));

            // Check if the item is a currency and handle it accordingly
            if (rewardItems[i].itemType != ItemType.Currency)
            {
                // If it's not currency, add it to the inventory
                InventoryManager.Instance.AddItem(rewardItems[i], rewardAmounts[i]);
            }
        }

        // Handle currency separately
        for (int i = 0; i < currencies.Length; i++)
        {
            if (i < rewardAmounts.Count)  // Ensure there's a matching entry in rewardAmounts for this currency
            {
                int amount = rewardAmounts[i];
               // TrophyRecordUI.Instance.AddGoldToTrophy(amount);
                if (currencies[i] == CurrencyType.Coin)
                {
                    // Add coin to the Coin UI section (like the coin icon)
                    CurrencyManager.Instance.AddCurrency(currencies[i], amount);
                }
                else
                {
                    // Handle other currencies if needed
                    CurrencyManager.Instance.AddCurrency(currencies[i], amount);
                }
            }
        }

        // Show the rewards in the UI
        if (rewardPanel != null)
        {
            rewardPanel.ShowRewards(rewards);
        }
        else
        {
            Debug.LogWarning("RewardPanel chưa được gán vào ChestController!");
        }
    }
}