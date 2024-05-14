using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dialogue : MonoBehaviour
{
    [SerializeField] protected string[] sDialogues;
    [SerializeField, Range(0f, 1f)] protected float fTextSpeed;
    [SerializeField] protected AudioSource audioSource;

    protected MasterDialogueController dialogueController;

    public MasterDialogueController Controller { get => dialogueController; }

    protected bool isPlaying = false;
    protected bool skipCurrentText = false;

    protected void Awake()
    {
        if (!dialogueController)
            dialogueController = GameObject.FindObjectOfType<MasterDialogueController>();
    }

    public void StartText()
    {
        StartCoroutine(playText());
    }

    protected IEnumerator playText()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            dialogueController.SpImage = true;
            foreach (string text in sDialogues)
            {
                dialogueController.Text.text = "";
                foreach (char ch in text)
                {
                    if (skipCurrentText)
                    {
                        skipCurrentText = false;
                        break;
                    }
                    dialogueController.Text.text += ch;
                    audioSource.Play();
                    yield return new WaitForSeconds(fTextSpeed);
                }
                dialogueController.Text.text = text;
                dialogueController.Skip.SetActive(!skipCurrentText);
                while (!skipCurrentText) yield return null;
                skipCurrentText = false;
                dialogueController.Skip.SetActive(skipCurrentText);
            }
            isPlaying = false;
            dialogueController.SpImage = false;
            dialogueController.Text.text = "";

            AfterTextDisplay();
        }
    }

    public abstract void Activate(bool state);
    public virtual void AfterTextDisplay() { }

    private void Update()
    {
        skipCurrentText = Input.GetKeyDown(KeyCode.Space) && isPlaying;
    }
}
