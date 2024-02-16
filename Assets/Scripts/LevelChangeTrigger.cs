using LevelManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers;

[RequireComponent(typeof(Collider2D))]
public class LevelChangeTrigger : MonoBehaviour
{
    [SerializeField] private string level;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            GameLevelManager.Instance.CallGameManager(level);
        }
    }
}
