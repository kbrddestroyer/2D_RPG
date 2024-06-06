using GameControllers;
using LevelManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SocialPlatforms.Impl;

[Singleton]
public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public static InventoryManager Instance { get => instance; }

    private List<Item> items = new List<Item>();
    private List<QuestItem> quests = new List<QuestItem>();
    
    private List<Item> itemsOnLoad;

    public bool Contains(Item item)
    {
        return items.Contains(item);
    }

    public void StartQuest(QuestItem quest)
    {
        quests.Add(quest);
        quest.StartQuest();
        GameLevelManager.Instance.AddQuestToGUI(quest);
    }

    public virtual void DeactivateQuest(QuestItem quest)
    {
        items.Add(quest.Reward);
        quest.FinalizeQuest();
        GameLevelManager.Instance.RebuildQuests();
    }

    public void DeactivateQuest(int index)
    {
        Assert.IsTrue(index >= 0 && index < quests.Count);
        QuestItem quest = quests[index];

        items.Add(quest.Reward);
        GameLevelManager.Instance.RebuildQuests();
    }

    public void AddItem(Item item)
    {
        Items.Add(item);
        GameLevelManager.Instance.AddItem(item);
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
        GameLevelManager.Instance.RebuildItems();
    }

    public void NotifyOnSpawn()
    {
        itemsOnLoad = new List<Item>(items);
    }

    public void NotifyOnPlayerDeath()
    {
        items = new List<Item>(itemsOnLoad);
    }

    public List<Item> Items { get => items; }
    public List<Item> ItemsOnLoad { get => itemsOnLoad; }
    public List<QuestItem> Quests { get => quests; }

    protected void Awake()
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
}
