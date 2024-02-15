using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LevelManagement
{
    [Singleton]
    public class GameLevelManager : MonoBehaviour
    {
        /*
         * 
         *      GameLevelManager check that scene has every required singleton items
         * 
         */
        private static GameLevelManager instance;
        public static GameLevelManager Instance { get => instance; }
        [SerializeField] private GameManager gameManagerPrefab;
        [SerializeField] private InventoryManager inventoryManagerPrefab;
        [SerializeField] private InventoryItem itemPrefab;
        [Header("Level settings")]
        [SerializeField] private Transform tInventoryListRoot;
        [SerializeField] private GameObject tInventoryGUI;
        public Transform Root { get => tInventoryListRoot; }

        //[Header("GUI for inventory manager")]

        private void ToggleGUI(bool bState)
        {
            tInventoryGUI.SetActive(bState);
        }

        private void Awake()
        {
            if (GameManager.Instance == null)
                Instantiate(gameManagerPrefab);
            if (InventoryManager.Instance == null)
                Instantiate(inventoryManagerPrefab);

            foreach (Item item in InventoryManager.Instance.Items)
            {
                InventoryItem iItem = Instantiate(itemPrefab, Root);
                iItem.ItemSettings = item;
            }

            instance = this;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleGUI(!tInventoryGUI.activeInHierarchy);
            }
        }

        public void CallGameManager(string sceneName)
        {
            GameManager.Instance.Load(sceneName);
        }
    }
}