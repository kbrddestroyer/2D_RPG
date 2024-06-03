using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Obtain Item Achievement")]
public class ObtainItemAchievement : Achievement
{
    [SerializeField] private Item[] items;

    protected override bool validatePredicate()
    {
        foreach (Item item in items)
            if (!InventoryManager.Instance.Contains(item))
                return false;
        return true;
    }
}
