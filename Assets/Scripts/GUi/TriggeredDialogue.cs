using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDialogue : Dialogue, IMasterDialogue
{
    [SerializeField] private bool destroyAfter;
    private bool subscribed = false;

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
            if (isPlaying)
            {
                if (!MasterDialogueController.Instance.Skip.activeInHierarchy) MasterDialogueController.Instance.Skip.SetActive(true);
            }
            MasterDialogueController.Instance.Subscribe(this);
        }
    }

    public void Unsubscribe()
    {
        if (subscribed)
        {
            subscribed = false;
            if (textDisplayCoroutine != null)
                StopAllCoroutines();
            isPlaying = false;
            MasterDialogueController.Instance.Text.text = "";
            MasterDialogueController.Instance.Unsubscribe(this);
        }
    }

    public void Interact()
    {
        StartTextDisplay();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
