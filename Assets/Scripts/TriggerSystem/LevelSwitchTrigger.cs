using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameControllers;
using LevelManagement;

public class LevelSwitchTrigger : Trigger
{
    [SerializeField] private string levelName;
    [SerializeField] private Item[] itemsToUnlock;
    [SerializeField] private int spawnIDOnNextLevel;

    protected override void Action()
    {
        InventoryManager manager = InventoryManager.Instance;

        foreach (Item item in itemsToUnlock)
        {
            if (!manager.Contains(item))
                return;
        }

        foreach (Item item in itemsToUnlock)
            manager.RemoveItem(item);
        GameManager.Instance.Spawn = spawnIDOnNextLevel;
        GameLevelManager.Instance.CallGameManager(levelName);
    }

    protected override void Deactivate()
    {
    }
}
