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

    private int refCount = 0;
    public bool Enabled
    {
        get => Root.gameObject.activeInHierarchy;
        set
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
}
