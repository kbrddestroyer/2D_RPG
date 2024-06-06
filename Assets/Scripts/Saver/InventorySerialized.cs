using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySerialized
{
    [SerializeField] private int level;

    [SerializeField] private List<int> items = new List<int>();
    [SerializeField] private List<int> questsInProgress = new List<int>();
    [SerializeField] private List<int> questsCompleted = new List<int>();
    [SerializeField] private List<int> achievements = new List<int>();

    public List<int> Items { get => items; }
    public List<int> Quests { get => questsInProgress; }
    public List<int> QuestsCompleted { get => questsCompleted; }
    public List<int> Achievements { get => achievements; }
}
