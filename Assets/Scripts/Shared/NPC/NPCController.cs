using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    public AdvancedDialogueProfile profile;
    public DialogueManager dialogueManager;
    public RewardPanelUI rewardPanel; // Kéo panel này vào Inspector

    // Đặt đúng quest ID đầu tiên của bạn
    private const string firstQuestId = "main_1_crystal";

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
                QuestData questData = QuestManager.Instance.GetQuestById(profile.questId);

                if (questData != null && questData.requiredNpcIds != null &&
                    System.Array.Exists(questData.requiredNpcIds, id => id == profile.npcId))
                {
                    if (!QuestManager.Instance.HasTalkedWithNpc(profile.questId, profile.npcId))
                    {
                        dialogueManager.StartDialogueFromLines(
                            profile.questSpecialLines,
                            profile.characterName,
                            profile.avatar,
                            () =>
                            {
                                // --- CODE TRAO THƯỞNG ĐẶT Ở ĐÂY ---
                                if (profile.rewardList != null && profile.rewardList.Count > 0)
                                {
                                    Debug.Log($"[DEBUG] Trao thưởng khi kết thúc đối thoại NPC: {profile.characterName}");
                                    foreach (var reward in profile.rewardList)
                                    {
                                        if (reward.item != null && reward.amount > 0)
                                        {
                                            InventoryManager.Instance.AddItem(reward.item, reward.amount);
                                            Debug.Log($"[DEBUG] Add item: {reward.item?.itemName} x{reward.amount}");
                                        }
                                    }

                                    var rewardPairs = new List<(ItemData, int)>();
                                    foreach (var reward in profile.rewardList)
                                    {
                                        if (reward.item != null && reward.amount > 0)
                                            rewardPairs.Add((reward.item, reward.amount));
                                    }
                                    Debug.Log("[DEBUG] Call ShowRewards()");
                                    rewardPanel.ShowRewards(rewardPairs);
                                }
                                // Đánh dấu đã nói chuyện NPC này
                                QuestManager.Instance.MarkTalkedWithNpc(profile.questId, profile.npcId);
                                FindObjectOfType<QuestUIController>()?.BuildQuestList();
                            }
                        );
                        return;
                    }
                }

                // Các trường hợp khác
                dialogueManager.StartDialogueFromLines(
                    profile.questInProgressLines,
                    profile.characterName,
                    profile.avatar
                );
                return;
            }

            // c. Đã đủ điều kiện hoàn thành nhưng chưa hoàn thành (readyToComplete)
            else if (QuestManager.Instance.IsQuestReadyToComplete(profile.questId))
            {
                if (profile.isQuestGiver)
                {
                    dialogueManager.StartDialogueFromLines(
                        profile.questCompletedLines.Length > 0 ? profile.questCompletedLines : profile.defaultLines,
                        profile.characterName,
                        profile.avatar,
                        () =>
                        {
                            QuestManager.Instance.CompleteQuest(profile.questId);
                            QuestManager.Instance.RemoveReadyToComplete(profile.questId);
                            FindObjectOfType<QuestUIController>()?.BuildQuestList();
                        }
                    );
                }
                else
                {
                    dialogueManager.StartDialogueFromLines(
                        profile.questInProgressLines,
                        profile.characterName,
                        profile.avatar
                    );
                }
                return;
            }

            // d. Đã hoàn thành quest này
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

        // 3. Thoại mặc định (không liên quan quest, NPC phụ ngoài quest)
        dialogueManager.StartDialogueFromLines(
            profile.defaultLines,
            profile.characterName,
            profile.avatar,
            () =>
            {
                // Nếu NPC này không liên quan quest, vẫn có thể trao thưởng
                if (QuestManager.Instance.IsQuestInProgress(firstQuestId)
                    && profile.rewardList != null
                    && profile.rewardList.Count > 0)
                {
                    Debug.Log($"[DEBUG] Trao thưởng NPC ngoài quest: {profile.characterName}");
                    foreach (var reward in profile.rewardList)
                    {
                        if (reward.item != null && reward.amount > 0)
                        {
                            InventoryManager.Instance.AddItem(reward.item, reward.amount);
                            Debug.Log($"[DEBUG] Add item: {reward.item?.itemName} x{reward.amount}");
                        }
                    }

                    var rewardPairs = new List<(ItemData, int)>();
                    foreach (var reward in profile.rewardList)
                    {
                        if (reward.item != null && reward.amount > 0)
                            rewardPairs.Add((reward.item, reward.amount));
                    }
                    Debug.Log("[DEBUG] Call ShowRewards()");
                    rewardPanel.ShowRewards(rewardPairs);
                }
            }
        );
    }
}
