using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueController : Dialogue, IMasterDialogue
{
    [SerializeField, Range(0f, 10f)] protected float fTriggerDistance;
    [Header("Gizmos")]
    [SerializeField] protected Color gizmoColor = new Color(0, 0, 0, 1);

    private bool subscribed = false;

    public float TriggerDistance
    {
        get => fTriggerDistance;
    }

    public override void Activate(bool state)
    {
        if (state)
        {
            Subscribe();
            dialogueController.Activate.SetActive(!isPlaying);
            if (isPlaying)
            {
                if (!dialogueController.Skip.activeInHierarchy) dialogueController.Skip.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartTextDisplay();
            }
        }
        else
        {
            Unsubscribe();
            if (!dialogueController.Enabled)
            {
                if (textDisplayCoroutine != null)
                    StopCoroutine(textDisplayCoroutine);
                isPlaying = false;
            }
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, fTriggerDistance);
    }
#endif
}
