using GameControllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Items/Healing Item")]
public class HealingItem : Item
{
    public float healingAmount;

    public override void OnClick()
    {
        Player player = FindObjectOfType<Player>();
        player.HP += healingAmount;
    }
}
