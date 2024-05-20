using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestItemGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text description;

    public string Label { set => label.text = value; }
    public string Description { set => description.text = value; }

    private QuestItem quest;

    public QuestItem QuestSettings
    {
        get => quest;
        set
        {
            quest = value;
            Label = value.Label;
            Description = value.Description;
        }
    }
}
