using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class Pickable : MonoBehaviour, IPickable
{
    /*
     *  Parent class for every usable item 
     */
    [SerializeField] private Item itemSettings;
    [Header("Hints")]
    [SerializeField] private GameObject hintActivate;
    [Header("Required")]
    [SerializeField] private InventoryItem guiPrefab;

    public bool hint { get => hintActivate.activeInHierarchy; set => hintActivate.SetActive(value); }

    public void Pickup()
    {
        InventoryItem item = Instantiate(guiPrefab, LevelManagement.GameLevelManager.Instance.Root);
        item.ItemSettings = itemSettings;
        InventoryManager.Instance.Items.Add(itemSettings);
        Destroy(this.gameObject);
    }
}
