using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Pickable : MonoBehaviour, IPickable
{
    /*
     *  Parent class for every usable item 
     */
    [SerializeField] private Item itemSettings;
    [Header("Required")]
    [SerializeField] private InventoryItem guiPrefab;
    
    private MasterDialogueController dialogueController;

    public bool hint
    {
        get => dialogueController.Activate.activeInHierarchy; 
        set
        {
            dialogueController.Activate.SetActive(value);
        }
    }

    private void Awake()
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
}
