using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameControllers;
using LevelManagement;

public class LevelSwitchTrigger : Trigger
{
    [SerializeField] private string levelName;

    protected override void Action()
    {
        GameLevelManager.Instance.CallGameManager(levelName);
    }

    protected override void Deactivate()
    {
    }
}
