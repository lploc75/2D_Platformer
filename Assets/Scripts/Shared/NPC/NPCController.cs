using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    public AdvancedDialogueProfile profile;
    public DialogueManager dialogueManager;
    public RewardPanelUI rewardPanel; // Kéo panel này vào Inspector
    public ShopManager shopManager;
    // Đặt đúng quest ID đầu tiên của bạn
    private const string firstQuestId = "main_1_crystal";
    private const string chiefId = "chief"; // Đổi nếu npcId của trưởng làng khác!
    public enum ShopType
    {
        None,
        HealerShop,
        WeaponShop,
        BlacksmithShop
    }

    public ShopType shopType;
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
            // a. Chưa nhận quest: hiện thoại mời nhận, xong tự nhận (chỉ dùng nếu KHÔNG auto nhận quest)
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

                // ======= THOẠI OFFER CHO CHIEF LẦN ĐẦU =======
                if (profile.npcId == chiefId && !QuestManager.Instance.HasTalkedWithNpc(profile.questId, chiefId))
                {
                    dialogueManager.StartDialogueFromLines(
                        profile.questOfferLines,
                        profile.characterName,
                        profile.avatar,
                        () =>
                        {
                            QuestManager.Instance.MarkTalkedWithNpc(profile.questId, chiefId);
                            FindObjectOfType<QuestUIController>()?.BuildQuestList();
                        }
                    );
                    return;
                }
                // ======= KHOÁC CHIEF TRƯỚC KHI GẶP NPC KHÁC =======
                if (profile.npcId != chiefId && !QuestManager.Instance.HasTalkedWithNpc(profile.questId, chiefId))
                {
                    // Chưa gặp trưởng làng, chỉ hiện thoại mặc định, không tick, không thưởng
                    dialogueManager.StartDialogueFromLines(
                        profile.defaultLines,
                        profile.characterName,
                        profile.avatar
                    );
                    return;
                }
                // ======= END =======

                // ĐÃ GẶP CHIEF, ĐẾN NPC PHỤ
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
    public void OpenShop()
    {
        // Only show the shop if the panel is not already active
        if (!shopManager.shopPanel.activeSelf)
        {
            if (shopType != ShopType.None) // Check that the NPC has a shop
            {
                shopManager.shopPanel.SetActive(true);

                // Based on the shop type, open the corresponding shop
                switch (shopType)
                {
                    case ShopType.HealerShop:
                        shopManager.OpenHealerShop();  // Open Healer Shop
                        break;
                    case ShopType.WeaponShop:
                        shopManager.OpenWeaponShop();  // Open Weapon Shop
                        break;
                    case ShopType.BlacksmithShop:
                        shopManager.OpenBlacksmithShop();  // Open Blacksmith Shop
                        break;
                    default:
                        Debug.LogError("Unknown shop type!");
                        break;
                }
                
            }
            else
            {
                Debug.LogWarning("NPC does not have a shop assigned.");
            }
        }
        else
        {
            // Hide the panel if it's already open
            shopManager.shopPanel.SetActive(false);
        }
    }

}
