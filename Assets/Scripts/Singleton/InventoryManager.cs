using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[Singleton]
public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public static InventoryManager Instance { get => instance; }

    private List<Item> items = new List<Item>();
    private List<QuestItem> quests = new List<QuestItem>();

    public bool Contains(Item item)
    {
        foreach (Item _item in items)
            if (item.id == _item.id)
                return true; 
        return false;
    }

    public void StartQuest(QuestItem quest)
    {
        quests.Add(quest);
    }

    public void DeactivateQuest(QuestItem quest)
    {
        items.Add(quest.Reward);
        quests.Remove(quest);
    }

    public void DeactivateQuest(int index)
    {
        Assert.IsTrue(index >= 0 && index < quests.Count);
        QuestItem quest = quests[index];

        items.Add(quest.Reward);
        quests.Remove(quest);
    }

    public List<Item> Items { get => items; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning($"Warning on: {this.name}: instance is not null (2 or more singleton items are on scene)");
        }
    }

    [Obsolete]
    public void Insert(Item item)
    {
        items.Add(item);
    }

    [Obsolete]
    public void Remove(Item item)
    {
        items.Remove(item);
    }
}
