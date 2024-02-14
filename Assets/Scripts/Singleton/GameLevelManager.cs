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
        [Header("Level settings")]
        [SerializeField] private Transform tInventoryListRoot;
        public Transform Root { get => tInventoryListRoot; }

        //[Header("GUI for inventory manager")]


        private void Awake()
        {
            if (GameManager.Instance == null)
                Instantiate(gameManagerPrefab);
            if (InventoryManager.Instance == null)
                Instantiate(inventoryManagerPrefab);

            instance = this;
        }

        public void CallGameManager(string sceneName)
        {
            GameManager.Instance.Load(sceneName);
        }
    }
}