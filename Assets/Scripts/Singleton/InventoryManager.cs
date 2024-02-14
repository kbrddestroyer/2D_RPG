using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Singleton]
public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public static InventoryManager Instance { get => instance; }

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
