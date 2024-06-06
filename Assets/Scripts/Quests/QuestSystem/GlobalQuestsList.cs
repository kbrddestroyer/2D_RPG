using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Global Quest List")]
public class GlobalQuestsList : ScriptableObject
{
    public List<QuestItem> quests = new List<QuestItem>();
}
