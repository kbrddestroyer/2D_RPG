using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerPatch : InventoryManager
{
    public new void AddItem(Item item)
    {
        Items.Add(item);
    }
}
