using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Singleton quản lý nhiệm vụ - kết nối ScriptableObject (QuestData, QuestDatabase)
/// </summary>
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Database tập hợp tất cả các QuestData")]
    public QuestDatabase questDatabase;

    // Trạng thái nhiệm vụ
    private HashSet<string> acceptedQuests = new HashSet<string>();
    private HashSet<string> completedQuests = new HashSet<string>();
    private HashSet<string> readyToCompleteQuests = new HashSet<string>(); // Quest đã xong mọi bước, chờ gặp lại questgiver

    // (Tuỳ chọn) Nếu quest yêu cầu gặp nhiều NPC, quản lý tiến trình qua từ điển
    private Dictionary<string, HashSet<string>> questNpcTalkedWith = new Dictionary<string, HashSet<string>>();

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Lấy QuestData từ questId
    public QuestData GetQuestById(string questId)
    {
        if (questDatabase == null) return null;
        return questDatabase.allQuests.Find(q => q.questId == questId);
    }

    // Nhận nhiệm vụ
    public void AcceptQuest(string questId)
    {
        if (!acceptedQuests.Contains(questId))
        {
            acceptedQuests.Add(questId);
            Debug.Log($"[QuestManager] Accepted quest: {questId}");
            // Reset tiến trình (nếu quest nhiều bước)
            if (!questNpcTalkedWith.ContainsKey(questId))
                questNpcTalkedWith[questId] = new HashSet<string>();
        }
    }

    // Hoàn thành nhiệm vụ
    public void CompleteQuest(string questId)
    {
        if (acceptedQuests.Contains(questId) && !completedQuests.Contains(questId))
        {
            completedQuests.Add(questId);
            Debug.Log($"[QuestManager] Completed quest: {questId}");
        }
    }

    // Trạng thái nhiệm vụ
    public bool IsQuestAccepted(string questId) => acceptedQuests.Contains(questId);
    public bool IsQuestInProgress(string questId) =>
        acceptedQuests.Contains(questId) && !completedQuests.Contains(questId) && !readyToCompleteQuests.Contains(questId);
    public bool IsQuestCompleted(string questId) => completedQuests.Contains(questId);
    public bool IsQuestReadyToComplete(string questId) => readyToCompleteQuests.Contains(questId);

    // Lấy tất cả quest đang nhận
    public IEnumerable<string> GetAllAcceptedQuestIds() => acceptedQuests;

    // Lấy trạng thái hiển thị ("Đang làm", "Chờ hoàn thành", "Đã hoàn thành", "Chưa nhận")
    public string GetQuestStatus(string questId)
    {
        if (IsQuestCompleted(questId)) return "Completed";
        if (IsQuestReadyToComplete(questId)) return "Ready to Complete";
        if (IsQuestInProgress(questId)) return "In Progress";
        return "Not Accepted";
    }

    // --- Hỗ trợ quest nhiều bước (gặp đủ NPC) ---

    // Đánh dấu đã nói chuyện với 1 NPC trong quest
    public void MarkTalkedWithNpc(string questId, string npcId)
    {
        if (!IsQuestAccepted(questId)) return;

        if (!questNpcTalkedWith.ContainsKey(questId))
            questNpcTalkedWith[questId] = new HashSet<string>();

        questNpcTalkedWith[questId].Add(npcId);

        // Kiểm tra nếu đã nói chuyện đủ các NPC yêu cầu -> sẵn sàng hoàn thành
        var questData = GetQuestById(questId);
        if (questData != null && questData.requiredNpcIds != null && questData.requiredNpcIds.Length > 0)
        {
            bool allMet = true;
            foreach (var requiredNpc in questData.requiredNpcIds)
            {
                if (!questNpcTalkedWith[questId].Contains(requiredNpc))
                {
                    allMet = false;
                    break;
                }
            }
            if (allMet && !readyToCompleteQuests.Contains(questId) && !IsQuestCompleted(questId))
            {
                readyToCompleteQuests.Add(questId); // Quest đã xong các bước, chờ gặp lại questgiver để hoàn thành chính thức
            }
        }
    }

    // Kiểm tra đã nói chuyện với NPC nào chưa trong 1 quest
    public bool HasTalkedWithNpc(string questId, string npcId)
    {
        return questNpcTalkedWith.ContainsKey(questId) && questNpcTalkedWith[questId].Contains(npcId);
    }

    // Khi complete chính thức, loại khỏi readyToCompleteQuests
    public void RemoveReadyToComplete(string questId)
    {
        if (readyToCompleteQuests.Contains(questId))
            readyToCompleteQuests.Remove(questId);
    }
}
