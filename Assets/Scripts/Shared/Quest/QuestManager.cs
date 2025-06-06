using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private HashSet<string> acceptedQuests = new HashSet<string>();
    private HashSet<string> completedQuests = new HashSet<string>();

    void Awake()
    {
        Instance = this;
    }

    public bool IsQuestAccepted(string questId) => acceptedQuests.Contains(questId);
    public bool IsQuestInProgress(string questId) => acceptedQuests.Contains(questId) && !completedQuests.Contains(questId);
    public bool IsQuestCompleted(string questId) => completedQuests.Contains(questId);

    public void AcceptQuest(string questId)
    {
        if (!acceptedQuests.Contains(questId))
            acceptedQuests.Add(questId);
    }

    public void CompleteQuest(string questId)
    {
        if (acceptedQuests.Contains(questId))
            completedQuests.Add(questId);
    }
}
