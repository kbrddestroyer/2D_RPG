using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest - Obtain Item")]
public class QuestObtainItem : QuestItem
{
    [SerializeField] private Item itemToObtain;

    public override bool questValidationPredicate()
    {
        return InventoryManager.Instance.Contains(itemToObtain);
    }

    public override void StartQuest()
    {
    }

    public override void FinalizeQuest()
    {
        InventoryManager.Instance.RemoveItem(itemToObtain);
    }
}
