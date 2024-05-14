using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggeredDialogue))]
public class DialogueTrigger : Trigger
{
    [SerializeField] private TriggeredDialogue dialogue;

    protected override void Action()
    {
        dialogue.Activate(true);
        dialogue.StartText();
    }

    protected override void Deactivate()
    {
        dialogue.Activate(false);
    }
}
