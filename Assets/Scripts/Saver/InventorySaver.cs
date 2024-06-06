using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventorySaver : MonoBehaviour
{
    [SerializeField] private GlobalItemsList itemList;
    [SerializeField] private GlobalQuestsList questsList;
    [SerializeField] private AchievementGlobalList achievements;

    private void Start()
    {
        load();
    }

    private void OnDestroy()
    {
        save();
    }

    private void save()
    {
        InventorySerialized levelState = new InventorySerialized();

        InventoryManager inventory = InventoryManager.Instance;

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/inventoryPersistence.dat", FileMode.OpenOrCreate);

        if (inventory.ItemsOnLoad == null)
            return;

        foreach (Item item in inventory.ItemsOnLoad)
        {
            levelState.Items.Add(itemList.items.IndexOf(item));
        }

        foreach (QuestItem quest in inventory.Quests)
        {
            levelState.Quests.Add(questsList.quests.IndexOf(quest));
        }

        for (int i = 0; i < questsList.quests.Count; i++)
        {
            if (questsList.quests[i].questCompleted)
            {
                levelState.QuestsCompleted.Add(i);
            }
        }

        foreach (Achievement achievement in achievements.achievements)
        {
            if (achievement.Unlocked)
            {
                levelState.Achievements.Add(achievements.achievements.IndexOf(achievement));
            }
        }

        binaryFormatter.Serialize(file, levelState);
        file.Close();
    }

    private void load()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/inventoryPersistence.dat", FileMode.Open);

        InventorySerialized levelState = binaryFormatter.Deserialize(file) as InventorySerialized;
        foreach (int item in levelState.Items)
        {
            InventoryManager.Instance.AddItem(itemList.items[item]);
        }

        foreach (int questID in levelState.Quests)
        {
            QuestItem questItem = questsList.quests[questID];
            InventoryManager.Instance.StartQuest(questItem);
        }

        foreach (int questID in levelState.QuestsCompleted)
        {
            questsList.quests[questID].questStarted = true;
            questsList.quests[questID].questCompleted = true;
        }

        foreach (int achievementID in levelState.Achievements)
        {
            achievements.achievements[achievementID].Unlocked = true;
        }

        file.Close();
    }
}
