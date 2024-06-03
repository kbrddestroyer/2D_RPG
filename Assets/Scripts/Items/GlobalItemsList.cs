using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/GlobalItemList")]
public class GlobalItemsList : ScriptableObject
{
    public Item[] items;
}