using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class QuestData
{
    public string questName;
    public bool isAccepted;      // Đã nhận nhiệm vụ chưa?
    public bool isCompleted;     // Đã xong nhiệm vụ chưa?
    public PlayerController playerController; // Kéo Player vào Inspector

    [TextArea(2, 5)] public string[] dialogueBeforeAccept;   // Thoại khi chưa nhận
    [TextArea(2, 5)] public string[] dialogueOnProgress;     // Thoại khi đang làm
    [TextArea(2, 5)] public string[] dialogueCompleted;      // Thoại khi đã xong
}

public class NPCDialogueTrigger : MonoBehaviour
{
    public PlayerController playerController; // Kéo Player vào Inspector
    [Header("Dialogue & Quest Reference")]
    public DialogueManager dialogueManager;  // Kéo DialogueManager Panel vào đây
    public string npcName = "NPC";

    [Header("Talk Only (No Quest)")]
    [TextArea(2, 5)] public string[] defaultSentences; // Dành cho NPC không nhiệm vụ

    [Header("Quests (optional, can be empty)")]
    public List<QuestData> quests; // Có thể bỏ trống nếu NPC chỉ nói chuyện

    private bool playerInRange = false;

    void Update()
    {
        if (
            playerInRange
            && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            && playerController != null
            && playerController.IsMoving == false  // <- Kiểm tra player đứng yên
           )
        {
            // Nếu NPC có quest, ưu tiên xử lý quest
            if (quests != null && quests.Count > 0)
            {
                bool saidQuest = false;
                foreach (QuestData quest in quests)
                {
                    if (!quest.isAccepted)
                    {
                        dialogueManager.characterName = npcName;
                        dialogueManager.sentences = quest.dialogueBeforeAccept;
                        dialogueManager.StartDialogue();
                        quest.isAccepted = true; // Nhận nhiệm vụ luôn (hoặc tuỳ chỉnh)
                        saidQuest = true;
                        break;
                    }
                    else if (quest.isAccepted && !quest.isCompleted)
                    {
                        dialogueManager.characterName = npcName;
                        dialogueManager.sentences = quest.dialogueOnProgress;
                        dialogueManager.StartDialogue();
                        saidQuest = true;
                        break;
                    }
                    else if (quest.isCompleted)
                    {
                        dialogueManager.characterName = npcName;
                        dialogueManager.sentences = quest.dialogueCompleted;
                        dialogueManager.StartDialogue();
                        saidQuest = true;
                        break;
                    }
                }

                // Nếu không nói quest nào, nói thoại thường
                if (!saidQuest && defaultSentences != null && defaultSentences.Length > 0)
                {
                    dialogueManager.characterName = npcName;
                    dialogueManager.sentences = defaultSentences;
                    dialogueManager.StartDialogue();
                }
            }
            else
            {
                // NPC không có nhiệm vụ, chỉ nói chuyện thường
                if (defaultSentences != null && defaultSentences.Length > 0)
                {
                    dialogueManager.characterName = npcName;
                    dialogueManager.sentences = defaultSentences;
                    dialogueManager.StartDialogue();
                }
                else
                {
                    // Nếu chưa nhập thoại nào, nói câu mặc định
                    dialogueManager.characterName = npcName;
                    dialogueManager.sentences = new string[] { "Hi." };
                    dialogueManager.StartDialogue();
                }
            }
            playerInRange = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
