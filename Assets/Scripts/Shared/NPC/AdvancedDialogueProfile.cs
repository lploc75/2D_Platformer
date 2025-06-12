using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/AdvancedDialogueProfile")]
public class AdvancedDialogueProfile : ScriptableObject
{
    [Header("Thông tin NPC")]
    public string npcId;            // Đặt id duy nhất cho NPC này (ví dụ: "wizard", "blacksmith")
    public string characterName;
    public Sprite avatar;           // Ảnh đại diện NPC
    public bool isQuestGiver; // Chỉ tick cho trưởng làng (quest giver)


    [Header("Thoại mặc định (không liên quan quest)")]
    [TextArea(2, 5)] public string[] defaultLines;

    [Header("Quest liên quan NPC này")]
    public string questId;          // Nếu NPC có nhiệm vụ liên quan, điền questId

    [Header("Thoại mời nhận nhiệm vụ")]
    [TextArea(2, 5)] public string[] questOfferLines;

    [Header("Thoại khi nhiệm vụ đang làm (chưa đủ điều kiện hoặc đã nói chuyện rồi)")]
    [TextArea(2, 5)] public string[] questInProgressLines;

    [Header("Thoại đặc biệt khi nói chuyện lần đầu với NPC này trong quest")]
    [TextArea(2, 5)] public string[] questSpecialLines;   // VD: "Here is what I know about the crystal!"
    public ItemData rewardItem;     // Nếu muốn tặng đồ cho player khi nói chuyện lần đầu

    [Header("Thoại khi hoàn thành nhiệm vụ")]
    [TextArea(2, 5)] public string[] questCompletedLines;

    [Header("Thoại đặc biệt chỉ hiện khi đã hoàn thành quest khác")]
    public string unlockAfterQuestId; // ID nhiệm vụ cần hoàn thành trước để mở thoại này
    [TextArea(2, 5)] public string[] unlockedLines;
}
