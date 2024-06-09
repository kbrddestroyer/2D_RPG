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
    [SerializeField] private TMP_Text countLabel;
    [SerializeField] private Button button;

    private int count = 0;

    private Item item;

    public Sprite Icon { set => image.sprite = value; }
    public string Label { set => label.text = value; }
    public bool Usable { set => button.enabled = value; }
    public int Count
    {
        get => count;
        set
        {
            count = value;
            countLabel.text = count.ToString();
        }
    }

    private bool shouldDestroy()
    {
        Count--;

        InventoryManager.Instance.RemoveItem(item);

        return Count <= 0;
    }

    public Item ItemSettings
    {
        get => item;
        set {
            item = value;
            image.sprite = value.icon;
            label.text = value.label;
            button.gameObject.SetActive(value.isUsable);
            if (value.isUsable) {
                button.onClick.AddListener(() => { value.OnClick(); });
                button.onClick.AddListener(() => { if (shouldDestroy()) Destroy(this.gameObject); });
            }
        }
    }
}
