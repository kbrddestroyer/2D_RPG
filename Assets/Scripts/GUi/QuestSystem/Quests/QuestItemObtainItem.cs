using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestItemObtainItem : QuestItem
{
    [SerializeField] private Pickable itemToObtain;

    public override bool questValidationPredicate()
    {
        return InventoryManager.Instance.Contains(itemToObtain.ItemSetting);
    }
}
