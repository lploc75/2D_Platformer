using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class QuestUIController : MonoBehaviour
{
    public Transform questListParent;
    public GameObject questButtonPrefab;
    public TextMeshProUGUI questDetailText;



    public void BuildQuestList()
    {
        // Xóa toàn bộ button cũ
        foreach (Transform child in questListParent)
            Destroy(child.gameObject);

        var questIds = new List<string>(QuestManager.Instance.GetAllAcceptedQuestIds());
        questIds.Reverse(); // Quest mới nhất lên trên

        QuestData firstQuest = null;
        QuestListButton firstBtn = null;

        foreach (var questId in questIds)
        {
            QuestData quest = QuestManager.Instance.GetQuestById(questId);
            if (quest == null) continue;

            var btnObj = Instantiate(questButtonPrefab, questListParent);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = quest.questName; // Chỉ hiện tên quest

            // Set tick completed nếu quest đã hoàn thành
            var questBtn = btnObj.GetComponent<QuestListButton>();
            if (questBtn != null)
            {
                bool isCompleted = QuestManager.Instance.IsQuestCompleted(quest.questId);
                questBtn.SetCompleted(isCompleted);
            }

            QuestData capturedQuest = quest; // Tránh closure bug
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowQuestDetail(capturedQuest);
                // Đảm bảo button này được highlight khi bấm
                if (questBtn != null)
                {
                    foreach (Transform sibling in questListParent)
                    {
                        var siblingBtn = sibling.GetComponent<QuestListButton>();
                        if (siblingBtn != null) siblingBtn.SetSelected(false);
                    }
                    questBtn.SetSelected(true);
                }
            });

            // Gán quest đầu tiên (mới nhất) để lát nữa hiển thị chi tiết mặc định
            if (firstQuest == null)
            {
                firstQuest = quest;
                firstBtn = questBtn;
            }
        }

        // Hiện chi tiết quest mới nhất (trên cùng) mặc định, và highlight luôn nút đầu
        if (firstQuest != null)
        {
            ShowQuestDetail(firstQuest);
            if (firstBtn != null) firstBtn.SetSelected(true);
        }
        else
        {
            // Nếu không có quest, xóa text detail
            questDetailText.text = "";
        }
    }

    void ShowQuestDetail(QuestData quest)
    {
        questDetailText.text =
            $"<b>{quest.questName}</b>\n\n{quest.questDescription}\n\n" +
            $"<size=90%>Status: <color=yellow>{QuestManager.Instance.GetQuestStatus(quest.questId)}</color></size>";
    }
    void Start()
{
    BuildQuestList();
    QuestManager.Instance.OnQuestChanged += BuildQuestList;
}

void OnDestroy()
{
    if (QuestManager.Instance != null)
        QuestManager.Instance.OnQuestChanged -= BuildQuestList;
}

}
