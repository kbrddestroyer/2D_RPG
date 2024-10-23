using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest - Obtain Item")]
public class QuestObtainItem : QuestItem
{
    [Serializable]
    public struct ItemData
    {
        [SerializeField] public Item item;
        [SerializeField] public int amount;
    }

    [SerializeField] private ItemData[] itemsToObtain;

    public override bool questValidationPredicate()
    {
        foreach (ItemData item in itemsToObtain)
            if (!InventoryManager.Instance.Contains(item.item, item.amount))
                return false;
        return true;
    }

    public override void StartQuest()
    {
    }

    public override void FinalizeQuest()
    {
        foreach (ItemData item in itemsToObtain)
            if (InventoryManager.Instance.Contains(item.item, item.amount))
                InventoryManager.Instance.RemoveItem(item.item, item.amount);
    }
}
