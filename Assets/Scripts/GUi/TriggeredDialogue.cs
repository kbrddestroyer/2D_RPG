using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDialogue : Dialogue, IMasterDialogue
{
    [SerializeField] private bool destroyAfter;
    private bool subscribed = false;

    public override void Activate(bool state)
    {
        Debug.Log($"{name} {state}");
        if (!state)
        {
            Unsubscribe();
            dialogueController.Text.text = "";
            isPlaying = false;
            StopAllCoroutines();
        }
        else Subscribe();
        if (dialogueController.Skip != state) dialogueController.Skip.SetActive(state);
    }

    public override void AfterTextDisplay()
    {
        if (destroyAfter)
        {
            Destroy(gameObject);
        }
    }

    public void Subscribe()
    {
        if (!subscribed)
        {
            subscribed = true;
            dialogueController.Subscribe(this);
        }
    }

    public void Unsubscribe()
    {
        if (subscribed)
        {
            subscribed = false;
            dialogueController.Unsubscribe(this);
        }
    }
}
