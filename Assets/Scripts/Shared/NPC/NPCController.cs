using UnityEngine;

public class NPCController : MonoBehaviour
{
    public AdvancedDialogueProfile profile;
    public DialogueManager dialogueManager;

    public void Interact()
    {
        // Thoại đặc biệt: chỉ khi đã hoàn thành quest khác
        if (!string.IsNullOrEmpty(profile.unlockAfterQuestId) &&
            QuestManager.Instance.IsQuestCompleted(profile.unlockAfterQuestId))
        {
            dialogueManager.StartDialogueFromLines(profile.unlockedLines, profile.characterName, profile.avatar);
            return;
        }

        // Nếu NPC này có quest thì kiểm tra từng trạng thái
        if (!string.IsNullOrEmpty(profile.questId))
        {
            if (!QuestManager.Instance.IsQuestAccepted(profile.questId))
            {
                dialogueManager.StartDialogueFromLines(profile.questOfferLines, profile.characterName, profile.avatar);
                // Khi người chơi chọn nhận, nhớ gọi QuestManager.Instance.AcceptQuest(profile.questId);
                return;
            }
            else if (QuestManager.Instance.IsQuestInProgress(profile.questId))
            {
                dialogueManager.StartDialogueFromLines(profile.questInProgressLines, profile.characterName, profile.avatar);
                return;
            }
            else if (QuestManager.Instance.IsQuestCompleted(profile.questId))
            {
                dialogueManager.StartDialogueFromLines(profile.questCompletedLines, profile.characterName, profile.avatar);
                // Nếu là điểm trả nhiệm vụ, nhớ gọi QuestManager.Instance.CompleteQuest(profile.questId);
                return;
            }
        }

        // Thoại mặc định
        dialogueManager.StartDialogueFromLines(profile.defaultLines, profile.characterName, profile.avatar);
    }
}
