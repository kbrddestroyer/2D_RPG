using GameControllers;
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
        [SerializeField] private QuestItemGUI questItemPrefab;
        [Header("Level settings")]
        [SerializeField, AllowsNull] private Transform tInventoryListRoot;
        [SerializeField, AllowsNull] private Transform tQuestListRoot;
        [SerializeField] private GameObject tInventoryGUI;
        [Header("Player Spawn")]
        [SerializeField] private bool shouldSpawn = true;
        [SerializeField] private PlayableList playables;
        public Transform Root { get => tInventoryListRoot; }
        public Transform QuestRoot { get => tQuestListRoot; }

        private void ToggleGUI(bool bState)
        {
            tInventoryGUI.SetActive(bState);

            if (bState)
                RebuildItems();
        }

        public void AddQuestToGUI(QuestItem quest)
        {
            QuestItemGUI questItem = Instantiate(questItemPrefab, QuestRoot);
            questItem.QuestSettings = quest;
        }

        public void RebuildQuests()
        {
            foreach (QuestItemGUI questOld in FindObjectsOfType<QuestItemGUI>())
            {
                if (InventoryManager.Instance.Quests.Contains(questOld.QuestSettings) && !questOld.QuestSettings.questCompleted)
                    continue;
                Destroy(questOld.gameObject);
            }
        }

        public void AddItem(Item item)
        {
            InventoryItem iItem = Instantiate(itemPrefab, Root);
            iItem.ItemSettings = item;
        }

        public void RebuildItems()
        {
            foreach (InventoryItem itemOld in FindObjectsOfType<InventoryItem>())
            {
                if (InventoryManager.Instance.Items.Contains(itemOld.ItemSettings))
                    continue;
                Destroy(itemOld.gameObject);
            }
        }

        private void Awake()
        {
            if (GameManager.Instance == null)
                Instantiate(gameManagerPrefab);
            if (InventoryManager.Instance == null)
                Instantiate(inventoryManagerPrefab);

            if (shouldSpawn)
            {
                int id = PlayerPrefs.GetInt("playable", 0);
                Instantiate(playables.playable[id], transform.position, Quaternion.identity);
            }
                

            if (tInventoryListRoot == null)
                tInventoryListRoot = GameObject.FindGameObjectWithTag("InventoryRoot").transform;
            if (tQuestListRoot == null)
                tQuestListRoot = GameObject.FindGameObjectWithTag("QuestsRoot").transform;

            foreach (Item item in InventoryManager.Instance.Items)
            {
                AddItem(item);
            }
            RebuildQuests();
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