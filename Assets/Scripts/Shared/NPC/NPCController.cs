using UnityEngine;

public class NPCController : MonoBehaviour
{
    public AdvancedDialogueProfile profile;
    public DialogueManager dialogueManager;

    public void Interact()
    {
        // 1. Thoại đặc biệt mở khóa sau khi hoàn thành một quest khác (nếu có)
        if (!string.IsNullOrEmpty(profile.unlockAfterQuestId) &&
            QuestManager.Instance.IsQuestCompleted(profile.unlockAfterQuestId))
        {
            dialogueManager.StartDialogueFromLines(
                profile.unlockedLines,
                profile.characterName,
                profile.avatar
            );
            return;
        }

        // 2. Nếu NPC này có liên quan đến quest
        if (!string.IsNullOrEmpty(profile.questId))
        {
            // a. Chưa nhận quest: hiện thoại mời nhận, xong tự nhận
            if (!QuestManager.Instance.IsQuestAccepted(profile.questId))
            {
                // Chỉ NPC giao quest mới được cho nhận quest!
                if (profile.isQuestGiver)
                {
                    dialogueManager.StartDialogueFromLines(
                        profile.questOfferLines,
                        profile.characterName,
                        profile.avatar,
                        () =>
                        {
                            QuestManager.Instance.AcceptQuest(profile.questId);
                            FindObjectOfType<QuestUIController>()?.BuildQuestList();
                        }
                    );
                    return;
                }
                else
                {
                    // NPC phụ, chưa nhận quest, chỉ hiện thoại mặc định
                    dialogueManager.StartDialogueFromLines(
                        profile.defaultLines,
                        profile.characterName,
                        profile.avatar
                    );
                    return;
                }
            }


            // b. Đang làm quest này
            else if (QuestManager.Instance.IsQuestInProgress(profile.questId))
            {
                // Lấy dữ liệu quest
                QuestData questData = QuestManager.Instance.GetQuestById(profile.questId);

                // Nếu quest yêu cầu phải nói chuyện với NPC này (nằm trong requiredNpcIds)
                if (questData != null && questData.requiredNpcIds != null &&
                    System.Array.Exists(questData.requiredNpcIds, id => id == profile.npcId))
                {
                    // Nếu chưa nói chuyện với NPC này trong quest này
                    if (!QuestManager.Instance.HasTalkedWithNpc(profile.questId, profile.npcId))
                    {
                        // Thoại đặc biệt + đánh dấu đã nói chuyện + tặng item nếu có
                        dialogueManager.StartDialogueFromLines(
                            profile.questSpecialLines,
                            profile.characterName,
                            profile.avatar,
                            () =>
                            {
                                QuestManager.Instance.MarkTalkedWithNpc(profile.questId, profile.npcId);
                                //if (profile.rewardItem != null)
                                //{
                                //    InventoryManager.Instance.AddItem(profile.rewardItem);
                                //}
                            }
                        );
                        return;
                    }
                }

                // Đã nói chuyện rồi hoặc không thuộc danh sách NPC required, hiện thoại in-progress
                dialogueManager.StartDialogueFromLines(
                    profile.questInProgressLines,
                    profile.characterName,
                    profile.avatar
                );
                return;
            }

            // c. Đã hoàn thành quest này
            else if (QuestManager.Instance.IsQuestCompleted(profile.questId))
            {
                dialogueManager.StartDialogueFromLines(
                    profile.questCompletedLines.Length > 0 ? profile.questCompletedLines : profile.defaultLines,
                    profile.characterName,
                    profile.avatar
                );
                return;
            }
        }

        // 3. Thoại mặc định (không liên quan quest)
        dialogueManager.StartDialogueFromLines(
            profile.defaultLines,
            profile.characterName,
            profile.avatar
        );
    }
}
