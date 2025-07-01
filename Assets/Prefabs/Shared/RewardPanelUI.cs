using UnityEngine;
using System.Collections.Generic;

public class RewardPanelUI : MonoBehaviour
{
    public GameObject rewardIconPrefab;      // Kéo prefab RewardIcon vào đây
    public Transform rewardListContainer;    // Kéo object (panel) chứa các reward icon vào đây

    // Hàm này nhận vào danh sách phần thưởng, tạo UI động
    public void ShowRewards(List<(ItemData, int)> rewards)
    {
        // Xóa RewardIcon cũ nếu có
        foreach (Transform child in rewardListContainer)
            Destroy(child.gameObject);

        // Tạo mới các RewardIcon
        foreach (var reward in rewards)
        {
            GameObject go = Instantiate(rewardIconPrefab, rewardListContainer);
            RewardIconUI ui = go.GetComponent<RewardIconUI>();
            ui.Setup(reward.Item1, reward.Item2);
        }
    }
}
