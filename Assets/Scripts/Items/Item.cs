using GameControllers;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[Serializable]
[CreateAssetMenu(menuName = "Items/Base Item")]
public class Item : ScriptableObject
{
    [SerializeField] public Sprite icon;
    [SerializeField] public string label;
    [SerializeField] public bool isUsable;

    public virtual void OnClick() 
    { 
    }
}
