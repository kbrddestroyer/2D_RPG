using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueController : Dialogue
{
    [SerializeField, Range(0f, 10f)] protected float fTriggerDistance;
    [Header("Gizmos")]
    [SerializeField] protected Color gizmoColor = new Color(0, 0, 0, 1);

    public float TriggerDistance
    {
        get => fTriggerDistance;
    }

    public override void Activate(bool state)
    {
        dialogueController.Enabled = state;
        if (state)
        {
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
            if (textDisplayCoroutine != null)
                StopCoroutine(textDisplayCoroutine);
            isPlaying = false;
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
