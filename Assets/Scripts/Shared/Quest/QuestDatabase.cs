using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Quest/QuestDatabase")]
public class QuestDatabase : ScriptableObject
{
    public List<QuestData> allQuests;
}
