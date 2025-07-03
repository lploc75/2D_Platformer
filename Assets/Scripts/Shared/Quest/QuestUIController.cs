using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUIController : MonoBehaviour
{
    public Transform questListParent;
    public GameObject questButtonPrefab;
    public TextMeshProUGUI questDetailText;

    void Start()
    {
        BuildQuestList();
    }

    public void BuildQuestList()
    {
        // Xóa toàn bộ button cũ
        foreach (Transform child in questListParent)
            Destroy(child.gameObject);

        // Tạo lại list button cho các quest đã nhận
        foreach (var questId in QuestManager.Instance.GetAllAcceptedQuestIds())
        {
            QuestData quest = QuestManager.Instance.GetQuestById(questId);
            if (quest == null) continue;

            var btnObj = Instantiate(questButtonPrefab, questListParent);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text =
                $"{quest.questName}\n<size=80%><color=yellow></color></size>";

            // Set tick completed nếu quest đã hoàn thành
            var questBtn = btnObj.GetComponent<QuestListButton>();
            if (questBtn != null)
            {
                bool isCompleted = QuestManager.Instance.IsQuestCompleted(quest.questId);
                questBtn.SetCompleted(isCompleted);
            }

            QuestData capturedQuest = quest; // Để tránh closure bug
            btnObj.GetComponent<Button>().onClick.AddListener(() => ShowQuestDetail(capturedQuest));
        }
    }

    void ShowQuestDetail(QuestData quest)
    {
        questDetailText.text =
            $"<b>{quest.questName}</b>\n\n{quest.questDescription}\n\n" +
            $"<size=90%>Status: <color=yellow>{QuestManager.Instance.GetQuestStatus(quest.questId)}</color></size>";
    }
}
