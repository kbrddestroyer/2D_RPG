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
        if (state)
        {
            dialogueController.Activate.SetActive(!isPlaying);
            if (isPlaying)
            {
                if (!dialogueController.Skip.activeInHierarchy) dialogueController.Skip.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartText();
            }
        }
        else
        {
            if (!dialogueController.Enabled)
            {
                dialogueController.Text.text = "";
                dialogueController.Skip.SetActive(false);
                dialogueController.Activate.SetActive(false);
                dialogueController.SpImage = false;
            }
            StopAllCoroutines();
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
