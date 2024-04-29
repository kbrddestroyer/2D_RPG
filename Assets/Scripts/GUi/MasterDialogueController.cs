using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasterDialogueController : MonoBehaviour
{
    [SerializeField] private TMP_Text tText;
    [SerializeField] private Transform root;
    [SerializeField] private Image iImage;
    [Header("Hints")]
    [SerializeField] private GameObject hintActivate;
    [SerializeField] private GameObject hintSkip;

    public TMP_Text Text { get => tText; }
    public Transform Root { get => root; }
    public Image SpriteImage { get => iImage; }
    public GameObject Activate { get => hintActivate; }
    public GameObject Skip { get => hintSkip; }

    public bool Enabled
    {
        get => Root.gameObject.activeInHierarchy;
        set
        {
            Root.gameObject.SetActive(value);
        }
    }

    public bool SpImage
    {
        set
        {
            iImage.enabled = value;
        }
    }
}
