using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public Animator animator;
    public List<ItemData> rewardItems;
    public List<int> rewardAmounts;
    public RewardPanelUI rewardPanel;

    public bool isOpened = false;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        // Ở đây bạn cần gán lại isOpened nếu có hệ thống load riêng (vd: từ GameManager)
        if (rewardPanel == null)
            rewardPanel = FindFirstObjectByType<RewardPanelUI>();

    }

    private void Start()
    {
        // Nếu đã mở thì chuyển Animator về trạng thái "Opened"
        if (isOpened)
        {
            animator.Play("Opened"); // Đảm bảo "Opened" là tên state mở hoàn toàn
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
        // Ở đây bạn nên gọi hàm lưu trạng thái rương nếu có hệ thống save riêng

        ShowRewardPanel();
    }

    private void ShowRewardPanel()
    {
        List<(ItemData, int)> rewards = new List<(ItemData, int)>();
        for (int i = 0; i < rewardItems.Count; i++)
        {
            rewards.Add((rewardItems[i], rewardAmounts[i]));
            InventoryManager.Instance.AddItem(rewardItems[i], rewardAmounts[i]);
        }

        if (rewardPanel != null)
            rewardPanel.ShowRewards(rewards);
        else
            Debug.LogWarning("RewardPanel chưa được gán vào ChestController!");
    }
}
