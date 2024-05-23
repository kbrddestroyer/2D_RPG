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
    
    private MasterDialogueController dialogueController;
    private bool subscribed = false;

    public Item ItemSetting { get => itemSettings; }

    public bool hint
    {
        get => dialogueController.Activate.activeInHierarchy; 
        set
        {
            dialogueController.Activate.SetActive(value);
        }
    }

    private void Start()
    {
        if (!dialogueController)
            dialogueController = GameObject.FindObjectOfType<MasterDialogueController>();
    }

    public void Pickup()
    {
        InventoryItem item = Instantiate(guiPrefab, LevelManagement.GameLevelManager.Instance.Root);
        item.ItemSettings = itemSettings;
        InventoryManager.Instance.Items.Add(itemSettings);
        Destroy(this.gameObject);
    }

    public void Subscribe()
    {
        if (!subscribed)
        {
            subscribed = true;
            dialogueController.Activate.SetActive(true);
            dialogueController.Subscribe(this);
        }
    }

    public void Unsubscribe()
    {
        if (subscribed)
        {
            subscribed = false;
            dialogueController.Activate.SetActive(false);
            dialogueController.Unsubscribe(this);
        }
    }
}
