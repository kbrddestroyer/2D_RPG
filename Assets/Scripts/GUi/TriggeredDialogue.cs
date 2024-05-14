using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDialogue : Dialogue
{
    [SerializeField] private bool destroyAfter;

    public override void Activate(bool state)
    {
        Debug.Log($"{name} {state}");
        dialogueController.Enabled = state;
        if (!state)
        {
            dialogueController.Text.text = "";
            isPlaying = false;
            StopAllCoroutines();
        }
        if (dialogueController.Skip != state) dialogueController.Skip.SetActive(state);
    }

    public override void AfterTextDisplay()
    {
        if (destroyAfter)
        {
            Destroy(gameObject);
        }
    }
}
