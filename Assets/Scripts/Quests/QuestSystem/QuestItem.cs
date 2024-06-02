using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Base/Base Item (cannot be created)")]
public abstract class QuestItem : ScriptableObject
{
    [SerializeField] private string questLabel;
    [SerializeField, Multiline] private string questDescription;
    [SerializeField] private Item reward;
    [SerializeField] private int id;

    public bool questStarted;
    public bool questCompleted;

    public Item Reward { get => reward; }

    public string Label { get => questLabel; }
    public string Description { get => questDescription; }

    public bool Started { get => questStarted; }
    public bool Completed { get => questCompleted; }    
    public int ID { get => id; }

    public abstract bool questValidationPredicate();

    public abstract void StartQuest();
    public abstract void FinalizeQuest();
}
