using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class InventorySaver : MonoBehaviour
{
    [SerializeField] private GlobalItemsList itemList;
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

        foreach (Item item in inventory.Items)
        {
            levelState.Items.Add(itemList.items.IndexOf(item));
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

        file.Close();
    }
}
