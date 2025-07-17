using UnityEngine;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    private static HashSet<string> questCompletedTalked = new HashSet<string>();
    public AdvancedDialogueProfile profile;
    public DialogueManager dialogueManager;
    public RewardPanelUI rewardPanel; // Kéo panel này vào Inspector
    public ShopManager shopManager;
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
        Debug.Log($"[NPCController] Interact() CALLED - IsDialoguePlaying: {dialogueManager.IsDialoguePlaying}");

        if (dialogueManager.IsDialoguePlaying)
        {
            Debug.Log("[NPCController] Dialogue đang chạy, bỏ qua tương tác.");
            return;
        }

        // 1. Thoại đặc biệt mở khóa sau khi hoàn thành một quest khác (nếu có)
        if (!string.IsNullOrEmpty(profile.unlockAfterQuestId) &&
            QuestManager.Instance.IsQuestCompleted(profile.unlockAfterQuestId))
        {
            Debug.Log("[NPCController] Thoại mở khoá sau quest khác.");
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
            // a. Chưa nhận quest
            if (!QuestManager.Instance.IsQuestAccepted(profile.questId))
            {
                Debug.Log("[NPCController] Chưa nhận quest.");
                if (profile.isQuestGiver)
                {
                    Debug.Log("[NPCController] Là questgiver, mời nhận quest.");
                    dialogueManager.StartDialogueFromLines(
                        profile.questOfferLines,
                        profile.characterName,
                        profile.avatar,
                        () =>
                        {
                            Debug.Log("[NPCController] Callback nhận quest!");
                            QuestManager.Instance.AcceptQuest(profile.questId);
                            FindObjectOfType<QuestUIController>()?.BuildQuestList();
                            // **KHÔNG GỌI SAVE Ở ĐÂY** (chỉ nhận quest, chưa phải trạng thái ổn định!)
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
                Debug.Log("[NPCController] Đang làm quest.");
                QuestData questData = QuestManager.Instance.GetQuestById(profile.questId);

                // ======= THOẠI OFFER CHO CHIEF LẦN ĐẦU =======
                if (profile.npcId == chiefId && !QuestManager.Instance.HasTalkedWithNpc(profile.questId, chiefId))
                {
                    Debug.Log("[NPCController] Lần đầu nói chuyện với chief.");
                    dialogueManager.StartDialogueFromLines(
                        profile.questOfferLines,
                        profile.characterName,
                        profile.avatar,
                        () =>
                        {
                            Debug.Log("[NPCController] Đã nói chuyện với chief (quest step).");
                            QuestManager.Instance.MarkTalkedWithNpc(profile.questId, chiefId);
                            FindObjectOfType<QuestUIController>()?.BuildQuestList();
                            // KHÔNG save ở đây (vẫn đang làm quest)
                        }
                    );
                    return;
                }
                // ======= KHOÁC CHIEF TRƯỚC KHI GẶP NPC KHÁC =======
                if (profile.npcId != chiefId && !QuestManager.Instance.HasTalkedWithNpc(profile.questId, chiefId))
                {
                    Debug.Log("[NPCController] NPC phụ, chưa gặp chief.");
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
                        Debug.Log("[NPCController] NPC phụ trong requiredNpcIds, chuẩn bị trao thưởng.");
                        dialogueManager.StartDialogueFromLines(
                            profile.questSpecialLines,
                            profile.characterName,
                            profile.avatar,
                            () =>
                            {
                                Debug.Log("[NPCController] Callback NPC phụ trao thưởng.");
                                if (profile.rewardList != null && profile.rewardList.Count > 0)
                                {
                                    foreach (var reward in profile.rewardList)
                                    {
                                        if (reward.item != null && reward.amount > 0)
                                        {
                                            InventoryManager.Instance.AddItem(reward.item, reward.amount);
                                            Debug.Log($"[NPCController] Add item: {reward.item?.itemName} x{reward.amount}");
                                        }
                                    }

                                    var rewardPairs = new List<(ItemData, int)>();
                                    foreach (var reward in profile.rewardList)
                                    {
                                        if (reward.item != null && reward.amount > 0)
                                            rewardPairs.Add((reward.item, reward.amount));
                                    }
                                    rewardPanel.ShowRewards(rewardPairs);
                                }
                                QuestManager.Instance.MarkTalkedWithNpc(profile.questId, profile.npcId);
                                FindObjectOfType<QuestUIController>()?.BuildQuestList();
                                // KHÔNG SAVE ở đây!
                            }
                        );
                        return;
                    }
                }

                // Các trường hợp khác
                Debug.Log("[NPCController] Quest in progress, thoại in progress.");
                dialogueManager.StartDialogueFromLines(
                    profile.questInProgressLines,
                    profile.characterName,
                    profile.avatar
                );
                return;
            }
            // c. Quest ready to complete
            else if (QuestManager.Instance.IsQuestReadyToComplete(profile.questId))
            {
                Debug.Log("[NPCController] Quest ready to complete.");
                if (profile.isQuestGiver)
                {
                    dialogueManager.StartDialogueFromLines(
                        profile.questCompletedLines.Length > 0 ? profile.questCompletedLines : profile.defaultLines,
                        profile.characterName,
                        profile.avatar,
                        () =>
                        {
                            Debug.Log("[NPCController] Callback hoàn thành quest!");
                            QuestManager.Instance.CompleteQuest(profile.questId);
                            QuestManager.Instance.RemoveReadyToComplete(profile.questId);
                            FindObjectOfType<QuestUIController>()?.BuildQuestList();

                            if (profile.questId == "main_1_crystal")
                            {
                                Debug.Log("[NPCController] Nhận quest tiếp theo: main_2_crystal");
                                QuestManager.Instance.AcceptQuest("main_2_crystal");
                                FindObjectOfType<QuestUIController>()?.BuildQuestList();

                                // ====== CHỈ GỌI SAVE Ở ĐÂY SAU KHI ĐÃ NHẬN QUEST 2! ======
                                GameSaveManager.Instance.SaveGame();
                            }
                            else
                            {
                                // Với các quest khác, save sau khi complete và trả xong
                                GameSaveManager.Instance.SaveGame();
                            }

                            // ==== FIX ĐÁNH DẤU LUÔN ====
                            string key = profile.npcId + "_" + profile.questId;
                            if (!questCompletedTalked.Contains(key)) questCompletedTalked.Add(key);
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
                string key = profile.npcId + "_" + profile.questId;
                Debug.Log($"[NPCController] Đã hoàn thành quest. key={key}, talked={questCompletedTalked.Contains(key)}");

                if (!questCompletedTalked.Contains(key) && profile.questCompletedLines.Length > 0)
                {
                    questCompletedTalked.Add(key);
                    Debug.Log("[NPCController] Hiện thoại questCompletedLines lần đầu.");
                    dialogueManager.StartDialogueFromLines(
                        profile.questCompletedLines,
                        profile.characterName,
                        profile.avatar
                    );
                }
                else
                {
                    Debug.Log("[NPCController] Đã hiện thoại completed, chuyển sang thoại mặc định.");
                    dialogueManager.StartDialogueFromLines(
                        profile.defaultLines,
                        profile.characterName,
                        profile.avatar
                    );
                }
                return;
            }
        }

        // 3. Thoại mặc định (không liên quan quest, NPC phụ ngoài quest)
        Debug.Log("[NPCController] Thoại mặc định.");
        dialogueManager.StartDialogueFromLines(
            profile.defaultLines,
            profile.characterName,
            profile.avatar,
            () =>
            {
                if (QuestManager.Instance.IsQuestInProgress(firstQuestId)
                    && profile.rewardList != null
                    && profile.rewardList.Count > 0)
                {
                    Debug.Log($"[NPCController] Trao thưởng NPC ngoài quest: {profile.characterName}");
                    foreach (var reward in profile.rewardList)
                    {
                        if (reward.item != null && reward.amount > 0)
                        {
                            InventoryManager.Instance.AddItem(reward.item, reward.amount);
                            Debug.Log($"[NPCController] Add item: {reward.item?.itemName} x{reward.amount}");
                        }
                    }

                    var rewardPairs = new List<(ItemData, int)>();
                    foreach (var reward in profile.rewardList)
                    {
                        if (reward.item != null && reward.amount > 0)
                            rewardPairs.Add((reward.item, reward.amount));
                    }
                    Debug.Log("[NPCController] Call ShowRewards()");
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
