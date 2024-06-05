using System.Collections;
using System.Collections.Generic;
using GameControllers;
using LevelManagement;
using NUnit.Framework;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.TestTools;

public class SaverTest
{
    [Test]
    public void SaverTestSimplePasses()
    {
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator InventoryManagerRuntimeTest()
    {
        GameObject.Instantiate(new GameObject()).AddComponent<InventoryManager>();
        new GameLevelManager();

        yield return new WaitForEndOfFrame();

        Assert.NotNull(InventoryManager.Instance);

        Item item = ScriptableObject.CreateInstance<Item>();

        item.name = "Test Item";
        item.label = "Test";
        item.isUsable = false;
        InventoryManager.Instance.Items.Add(item);
        Assert.IsTrue(InventoryManager.Instance.Items.Contains(item));
    }
}
