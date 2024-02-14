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

        [SerializeField] private GameManager gameManagerPrefab;
        [SerializeField] private InventoryManager inventoryManagerPrefab;

        //[Header("GUI for inventory manager")]


        private void Awake()
        {
            if (GameManager.Instance == null)
                Instantiate(gameManagerPrefab);
            if (InventoryManager.Instance == null)
                Instantiate(inventoryManagerPrefab);
        }
    }
}