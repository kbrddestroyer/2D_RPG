using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDialogue : Dialogue
{
    public override void Activate(bool state)
    {
        Debug.Log($"{name} {state}");
        dialogueController.Enabled = state;
        if (!state)
        {
            dialogueController.Text.text = "";
            isPlaying = false;
        }
        if (dialogueController.Skip != state) dialogueController.Skip.SetActive(state);
    }
}
