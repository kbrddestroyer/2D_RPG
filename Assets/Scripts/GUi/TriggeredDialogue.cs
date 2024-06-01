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
            MasterDialogueController.Instance.Text.text = "";
            isPlaying = false;
            StopAllCoroutines();
        }
        else Subscribe();
        if (MasterDialogueController.Instance.Skip != state) MasterDialogueController.Instance.Skip.SetActive(state);
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
            MasterDialogueController.Instance.Subscribe(this);
        }
    }

    public void Unsubscribe()
    {
        if (subscribed)
        {
            subscribed = false;
            MasterDialogueController.Instance.Unsubscribe(this);
        }
    }
}
