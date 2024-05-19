using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TalkerBase : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] protected float fTextSpeed;
    [SerializeField] protected AudioSource audioSource;

    protected bool isPlaying = false;
    protected bool skipCurrentText = false;
    protected Coroutine textDisplayCoroutine = null;

    protected MasterDialogueController dialogueController;
    public MasterDialogueController Controller { get => dialogueController; }

    protected virtual void Awake()
    {
        if (!dialogueController)
            dialogueController = GameObject.FindObjectOfType<MasterDialogueController>();
    }

    public void StartText(string[] sDialogues)
    {
        textDisplayCoroutine = StartCoroutine(playText(sDialogues));
    }

    protected IEnumerator playText(string[] sDialogues)
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

    public abstract void AfterTextDisplay();

    public abstract void Activate(bool state);

    protected virtual void Update()
    {
        skipCurrentText = Input.GetKeyDown(KeyCode.Space) && isPlaying;
    }
}
