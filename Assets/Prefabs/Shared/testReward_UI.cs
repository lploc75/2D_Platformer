using UnityEngine;
using System.Collections.Generic;

public class TestRewardUI : MonoBehaviour
{
    public RewardPanelUI rewardPanelUI;
    public List<ItemData> testItems; // Kéo các ItemData asset vào đây trong Inspector

    void Start()
    {
        var rewards = new List<(ItemData, int)>
        {
            (testItems[0], 1),
            (testItems[1], 3),
            (testItems[2], 2)
        };
        rewardPanelUI.ShowRewards(rewards);
    }
}
