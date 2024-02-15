using LevelManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text label;
    [SerializeField] private Button button;

    public Sprite Icon { set => image.sprite = value; }
    public string Label { set => label.text = value; }
    public bool Usable { set => button.enabled = value; }

    public Button.ButtonClickedEvent onClick { set => button.onClick = value; }

    public Item ItemSettings
    {
        set {
            image.sprite = value.icon;
            label.text = value.label;
            button.enabled = value.isUsable;
            if (value.isUsable)
                button.onClick = value.onClick;
        }
    }
}
