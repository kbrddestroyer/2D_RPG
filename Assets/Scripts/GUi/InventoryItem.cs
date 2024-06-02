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

    [HideInInspector] public int id;

    private Item item;

    public Sprite Icon { set => image.sprite = value; }
    public string Label { set => label.text = value; }
    public bool Usable { set => button.enabled = value; }

    public Item ItemSettings
    {
        get => item;
        set {
            item = value;
            id = value.id;
            image.sprite = value.icon;
            label.text = value.label;
            button.gameObject.SetActive(value.isUsable);
            if (value.isUsable) {
                button.onClick.AddListener(() => { value.OnClick(); });
                button.onClick.AddListener(() => { Destroy(this.gameObject); });
            }
        }
    }
}
