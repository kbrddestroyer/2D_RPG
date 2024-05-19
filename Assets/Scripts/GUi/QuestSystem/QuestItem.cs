using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class QuestItem
{
    [SerializeField] private string questLabel;
    [SerializeField] private Item reward;

    [HideInInspector] public bool questStarted;
    [HideInInspector] public bool questCompleted;

    public Item Reward { get => reward; }

    public abstract bool questValidationPredicate();
}
