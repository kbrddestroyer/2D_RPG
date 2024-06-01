using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Pickable : MonoBehaviour, IPickable, IMasterDialogue
{
    /*
     *  Parent class for every usable item 
     */
    [SerializeField] protected Item itemSettings;
    [Header("Required")]
    [SerializeField] protected InventoryItem guiPrefab;
    
    private bool subscribed = false;

    public Item ItemSetting { get => itemSettings; }

    public bool hint
    {
        get => MasterDialogueController.Instance.Activate.activeInHierarchy; 
        set
        {
            MasterDialogueController.Instance.Activate.SetActive(value);
        }
    }

    public void Pickup()
    {
        InventoryManager.Instance.AddItem(itemSettings);
        Destroy(this.gameObject);
    }

    public void Subscribe()
    {
        if (!subscribed)
        {
            subscribed = true;
            MasterDialogueController.Instance.Activate.SetActive(true);
            MasterDialogueController.Instance.Subscribe(this);
        }
    }

    public void Unsubscribe()
    {
        if (subscribed)
        {
            subscribed = false;
            MasterDialogueController.Instance.Activate.SetActive(false);
            MasterDialogueController.Instance.Unsubscribe(this);
        }
    }
}
