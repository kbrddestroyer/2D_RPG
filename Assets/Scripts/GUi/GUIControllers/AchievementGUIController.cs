using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementGUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text desription;

    public Achievement achievement
    {
        set
        {
            title.text = value.Title;
            desription.text = value.Description;
        }
    }
}
