using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIController : MonoBehaviour
{
    public Transform questListParent; // QuestMenu (content panel)
    public GameObject questButtonPrefab; // Prefab button mẫu (giống Button (1))
    public TextMeshProUGUI questDetailText; // Khung phải (TextMeshProUGUI)

    // Dữ liệu test quest (thay bằng list thật sau này)
    [System.Serializable]
    public class Quest
    {
        public string id;
        public string name;
        public string description;
        public string status;
    }
    public List<Quest> quests = new List<Quest>();

    void Start()
    {
        // Test demo (có thể thêm/xóa trong Inspector)
        if (quests.Count == 0)
        {
            quests.Add(new Quest { id = "q1", name = "Thu thập táo", description = "Hái 5 quả táo cho NPC.", status = "Đang làm" });
            quests.Add(new Quest { id = "q2", name = "Đánh bại Slime", description = "Tiêu diệt 3 Slime ở rừng.", status = "Chưa nhận" });
        }

        BuildQuestList();
    }

    void BuildQuestList()
    {
        // Clear quest cũ
        foreach (Transform child in questListParent)
            Destroy(child.gameObject);

        // Tạo mới từng dòng quest
        foreach (var quest in quests)
        {
            var btnObj = Instantiate(questButtonPrefab, questListParent);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = $"{quest.name}\n<size=80%><color=yellow>{quest.status}</color></size>";

            // Bắt sự kiện click
            Quest captured = quest; // Để tránh lỗi delegate
            btnObj.GetComponent<Button>().onClick.AddListener(() => ShowQuestDetail(captured));
        }
    }

    void ShowQuestDetail(Quest quest)
    {
        questDetailText.text = $"<b>{quest.name}</b>\n\n{quest.description}\n\n<size=90%>Trạng thái: <color=yellow>{quest.status}</color></size>";
    }
}
