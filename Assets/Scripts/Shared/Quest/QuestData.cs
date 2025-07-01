using UnityEngine;

[CreateAssetMenu(menuName = "Quest/QuestData")]
public class QuestData : ScriptableObject
{
    public string questId;
    public string questName;
    [TextArea] public string questDescription;

    public string[] requiredNpcIds;  // Nếu cần: danh sách NPC phải nói chuyện (để hoàn thành)
}
