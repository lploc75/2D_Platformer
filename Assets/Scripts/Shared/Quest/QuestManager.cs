using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Singleton quản lý nhiệm vụ - kết nối ScriptableObject (QuestData, QuestDatabase)
/// </summary>
public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestSaveData
    {
        public List<string> acceptedQuests = new List<string>();
        public List<string> completedQuests = new List<string>();
        public List<string> readyToCompleteQuests = new List<string>();
        public List<QuestNpcTalkedData> questNpcTalkedWith = new List<QuestNpcTalkedData>();
    }

    [System.Serializable]
    public class QuestNpcTalkedData
    {
        public string questId;
        public List<string> npcIds = new List<string>();
    }

    public static QuestManager Instance;

    [Header("Database tập hợp tất cả các QuestData")]
    public QuestDatabase questDatabase;

    // Trạng thái nhiệm vụ
    private HashSet<string> acceptedQuests = new HashSet<string>();
    private HashSet<string> completedQuests = new HashSet<string>();
    private HashSet<string> readyToCompleteQuests = new HashSet<string>(); // Quest đã xong mọi bước, chờ gặp lại questgiver

    // Nếu quest yêu cầu gặp nhiều NPC, quản lý tiến trình qua từ điển
    private Dictionary<string, HashSet<string>> questNpcTalkedWith = new Dictionary<string, HashSet<string>>();

    // === Quest mở đầu tự nhận khi start game ===
    [Header("ID nhiệm vụ mở đầu (nhận tự động)")]
    public string autoStartQuestId = "main_1_crystal";

    // Event cho NPC marker (và UI, nếu muốn)
    public event Action OnQuestChanged;

    void Awake()
    {
        // Singleton pattern + giữ lại khi load scene mới
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // KHÔNG bị phá hủy khi load scene mới!
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Tự động nhận quest mở đầu nếu chưa có
        if (!string.IsNullOrEmpty(autoStartQuestId) && !IsQuestAccepted(autoStartQuestId))
            AcceptQuest(autoStartQuestId);
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
            if (!questNpcTalkedWith.ContainsKey(questId))
                questNpcTalkedWith[questId] = new HashSet<string>();
            OnQuestChanged?.Invoke(); // Gọi event update marker
        }
    }

    // Hoàn thành nhiệm vụ
    public void CompleteQuest(string questId)
    {
        if (acceptedQuests.Contains(questId) && !completedQuests.Contains(questId))
        {
            completedQuests.Add(questId);
            readyToCompleteQuests.Remove(questId); // Đảm bảo không còn trạng thái ready
            Debug.Log($"[QuestManager] Completed quest: {questId}");
            OnQuestChanged?.Invoke(); // Gọi event update marker
        }
    }

    // Đánh dấu quest sẵn sàng hoàn thành (gặp lại questgiver để trả)
    public void SetQuestReadyToComplete(string questId)
    {
        if (!IsQuestAccepted(questId) || IsQuestCompleted(questId))
            return;
        if (!readyToCompleteQuests.Contains(questId))
        {
            readyToCompleteQuests.Add(questId);
            Debug.Log($"[QuestManager] Quest '{questId}' is now READY TO COMPLETE!");
            OnQuestChanged?.Invoke();
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
                Debug.Log($"[QuestManager] Quest '{questId}' is now READY TO COMPLETE (all NPCs)!");
            }
        }

        OnQuestChanged?.Invoke(); // Gọi event update marker
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

        OnQuestChanged?.Invoke(); // Gọi event update marker
    }

    // --- SAVE/LOAD DỮ LIỆU QUEST ĐỂ TÍCH HỢP VỚI GAME SAVE MANAGER ---

    public QuestSaveData GetSaveData()
    {
        QuestSaveData data = new QuestSaveData();
        data.acceptedQuests.AddRange(acceptedQuests);
        data.completedQuests.AddRange(completedQuests);
        data.readyToCompleteQuests.AddRange(readyToCompleteQuests);

        foreach (var pair in questNpcTalkedWith)
        {
            data.questNpcTalkedWith.Add(new QuestNpcTalkedData
            {
                questId = pair.Key,
                npcIds = new List<string>(pair.Value)
            });
        }
        return data;
    }

    public void LoadFromSaveData(QuestSaveData data)
    {
        if (data == null) return;
        acceptedQuests = new HashSet<string>(data.acceptedQuests);
        completedQuests = new HashSet<string>(data.completedQuests);
        readyToCompleteQuests = new HashSet<string>(data.readyToCompleteQuests);

        questNpcTalkedWith = new Dictionary<string, HashSet<string>>();
        foreach (var q in data.questNpcTalkedWith)
        {
            questNpcTalkedWith[q.questId] = new HashSet<string>(q.npcIds);
        }
        OnQuestChanged?.Invoke(); // Để update UI, marker
    }
}
