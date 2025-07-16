using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RewardInfo
{
    public ItemData item;
    public int amount = 1;
}

[CreateAssetMenu(menuName = "Dialogue/AdvancedDialogueProfile")]
public class AdvancedDialogueProfile : ScriptableObject
{
    [Header("Thông tin NPC")]
    public string npcId;
    public string characterName;
    public Sprite avatar;
    public bool isQuestGiver;

    [Header("Thoại mặc định (không liên quan quest)")]
    [TextArea(2, 5)] public string[] defaultLines;

    [Header("Quest liên quan NPC này")]
    public string questId;

    [Header("Thoại mời nhận nhiệm vụ")]
    [TextArea(2, 5)] public string[] questOfferLines;

    [Header("Thoại khi nhiệm vụ đang làm (chưa đủ điều kiện hoặc đã nói chuyện rồi)")]
    [TextArea(2, 5)] public string[] questInProgressLines;

    [Header("Thoại đặc biệt khi nói chuyện lần đầu với NPC này trong quest")]
    [TextArea(2, 5)] public string[] questSpecialLines;

    [Header("Phần thưởng khi nói chuyện (nếu có)")]
    public List<RewardInfo> rewardList;

    [Header("Thoại khi hoàn thành nhiệm vụ")]
    [TextArea(2, 5)] public string[] questCompletedLines;

    [Header("Thoại đặc biệt chỉ hiện khi đã hoàn thành quest khác")]
    public string unlockAfterQuestId;
    [TextArea(2, 5)] public string[] unlockedLines;
}