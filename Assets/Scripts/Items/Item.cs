using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class Item
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

    public override bool Equals(object obj)
    {
        if (obj is Item)
        {
            return ((Item) obj).id == id;
        }
        return false;
    }

    public virtual void OnClick() { }
}
