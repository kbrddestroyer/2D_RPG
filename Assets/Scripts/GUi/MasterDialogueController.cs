using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasterDialogueController : MonoBehaviour
{
    private static MasterDialogueController instance;
    public static MasterDialogueController Instance { get => instance; }

    [SerializeField] private TMP_Text tText;
    [SerializeField] private string text;
    [SerializeField] private Transform root;
    [SerializeField] private Image iImage;
    [Header("Hints")]
    [SerializeField] private GameObject hintActivate;
    [SerializeField] private GameObject hintSkip;

    public List<IMasterDialogue> subscribed = new List<IMasterDialogue>();

    public void Subscribe(IMasterDialogue ob)
    {
        Debug.Log($"Added {ob}");
        subscribed.Add(ob);
        Enabled = true;
    }

    public void Unsubscribe(IMasterDialogue ob)
    {
        Debug.Log($"Removed {ob}");
        if (subscribed.Remove(ob))
        {
            if (subscribed.Count == 0)
                setEnabled(false);
        }
    }

    public TMP_Text Text { get => tText; }
    public Transform Root { get => root; }
    public Image SpriteImage { get => iImage; }
    public GameObject Activate { get => hintActivate; }
    public GameObject Skip { get => hintSkip; }

    private int refCount = 0;
    public bool Enabled
    {
        get => Root.gameObject.activeInHierarchy;
        private set
        {
            refCount += (value) ? 1 : -1;
            
            if (refCount <= 0)
            {
                refCount = 0;
                setEnabled(false);
            }
            else setEnabled(true);
        }
    }

    private void setEnabled(bool value)
    {
        Root.gameObject.SetActive(value);

        if (!value)
        {
            Text.text = "";
            Skip.SetActive(false);
            Activate.SetActive(false);
            SpImage = false;
        }
    }

    public bool SpImage
    {
        set
        {
            iImage.enabled = value;
        }
    }

    private void Awake()
    {
        instance = this;
    }
}
