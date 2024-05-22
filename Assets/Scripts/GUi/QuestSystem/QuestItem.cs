using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class QuestItem
{
    [SerializeField] private string questLabel;
    [SerializeField, Multiline] private string questDescription;
    [SerializeField] private Item reward;
    [SerializeField] private int id;

    [HideInInspector] public bool questStarted;
    [HideInInspector] public bool questCompleted;

    public Item Reward { get => reward; }

    public string Label { get => questLabel; }
    public string Description { get => questDescription; }

    public bool Started { get => questStarted; }
    public bool Completed { get => questCompleted; }    
    public int ID { get => id; }

    public abstract bool questValidationPredicate();
}
