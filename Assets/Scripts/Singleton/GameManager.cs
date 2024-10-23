using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[Singleton]
public class GameManager : MonoBehaviour
{
    // Asynchronous scene loading
    // SINGLETON
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    private int selectedSpawn = 0;
    public int Spawn { get => selectedSpawn; set => selectedSpawn = value; }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Background task to load scene

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;
        
        while (operation.progress < 0.9f) 
            yield return null;

        operation.allowSceneActivation = true;
    } 

    private void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    public void Load(string sceneName)
    {
        LoadScene(sceneName);
    }

    public void Awake()
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
