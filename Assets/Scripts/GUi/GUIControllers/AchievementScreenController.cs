using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementScreenController : MonoBehaviour
{
    [SerializeField] private AchievementGlobalList achievements;
    [SerializeField] private AchievementGUIController gui;
    [SerializeField] private Transform guiRoot;

    [SerializeField] private string mainMenu;

    private void Start()
    {
        foreach (Achievement achievement in achievements.achievements)
        {
            Debug.Log(achievement.Title);
            if (achievement.Unlocked)
            {
                AchievementGUIController achievementGUI = Instantiate(gui, guiRoot.transform);
                achievementGUI.achievement = achievement;
            }
        }
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
