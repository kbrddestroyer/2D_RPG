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
    public int id;

    public static bool operator==(Item a, Item b)
    {
        return a.id == b.id;
    }
    public static bool operator !=(Item a, Item b)
    {
        return a.id != b.id;
    }
}
