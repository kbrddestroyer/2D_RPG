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
        GameLevelManager.Instance.CallGameManager(levelName);
    }

    protected override void Deactivate()
    {
    }
}
