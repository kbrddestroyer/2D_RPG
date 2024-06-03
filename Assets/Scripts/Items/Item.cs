using GameControllers;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "Items/Base Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string label;
    public bool isUsable;

    public virtual void OnClick() 
    { 
    }
}
