using GameControllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HealingItem : Item
{
    public float healingAmount;

    public override void OnClick()
    {
        Player player = GameObject.FindObjectOfType<Player>();
        player.HP += healingAmount;
    }
}
