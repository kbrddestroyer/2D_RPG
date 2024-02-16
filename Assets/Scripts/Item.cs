using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Item
{
    public Sprite icon;
    public string label;
    public bool isUsable;
    public Button.ButtonClickedEvent onClick;
}
