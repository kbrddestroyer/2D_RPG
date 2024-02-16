using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public struct Item
{
    public Sprite icon;
    public string label;
    public bool isUsable;
    public UnityEvent onClick;
}
