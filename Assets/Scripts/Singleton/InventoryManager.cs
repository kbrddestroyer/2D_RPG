using GameControllers;
using LevelManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEditor.Progress;

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
        Player.Instance.itemsPickedUpInLevel.Add(item);
        GameLevelManager.Instance.AddItem(item);
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
        Player.Instance.itemsPickedUpInLevel.Remove(item);
        GameLevelManager.Instance.RebuildItems();
    }

    public void NotifyOnPlayerDeath()
    {
        foreach (Item item in Player.Instance.itemsPickedUpInLevel)
            Items.Remove(item);
    }

    public List<Item> Items { get => items; }
    public List<QuestItem> Quests { get => quests; }

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
}
