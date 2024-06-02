using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggeredDialogue))]
public class DialogueTrigger : Trigger
{
    [SerializeField] private TriggeredDialogue dialogue;

    protected override void Action()
    {
        dialogue.Subscribe();
        dialogue.Interact();
    }

    protected override void Deactivate()
    {
        dialogue.Unsubscribe();
    }
}
