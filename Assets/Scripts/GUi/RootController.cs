using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RootController : MonoBehaviour
{
    private static RootController instance;
    public static RootController Instance { get => instance; }

    [SerializeField] private AchievementGUI achievement;

    private Queue<string> queued = new Queue<string>();

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator DequeueAll()
    {
        while (queued.Count > 0)
        {
            while (achievement.gameObject.activeInHierarchy)
                yield return new WaitForSeconds(1);
            achievement.title = queued.Dequeue();
            achievement.gameObject.SetActive(true);
        }
    }

    public void TriggerAchievement(string title)
    {
        queued.Enqueue(title);
        Debug.Log(title);
        StartCoroutine(DequeueAll());
    }
}
