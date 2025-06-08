using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/AdvancedDialogueProfile")]
public class AdvancedDialogueProfile : ScriptableObject
{
    [Header("Thông tin NPC")]
    public string characterName;
    public Sprite avatar; // Ảnh đại diện

    [Header("Thoại mặc định (chưa liên quan quest)")]
    [TextArea(2, 5)] public string[] defaultLines;

    [Header("Thoại mời nhận nhiệm vụ")]
    [TextArea(2, 5)] public string[] questOfferLines;
    public string questId;

    [Header("Thoại khi nhiệm vụ đang làm")]
    [TextArea(2, 5)] public string[] questInProgressLines;

    [Header("Thoại khi hoàn thành nhiệm vụ")]
    [TextArea(2, 5)] public string[] questCompletedLines;

    [Header("Thoại đặc biệt chỉ hiện khi đã hoàn thành quest khác")]
    public string unlockAfterQuestId; // ID nhiệm vụ cần hoàn thành trước
    [TextArea(2, 5)] public string[] unlockedLines;
}
