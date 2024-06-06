using GameControllers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueController : Dialogue, IMasterDialogue
{
    [SerializeField, Range(0f, 10f)] protected float fTriggerDistance;
    [SerializeField] private Item[] itemAfterDialogue;
    [SerializeField] private DialogueAchievement[] relatedAchievements;
    [Header("Gizmos")]
    [SerializeField] protected Color gizmoColor = new Color(0, 0, 0, 1);

    private bool subscribed = false;

    public float TriggerDistance
    {
        get => fTriggerDistance;
    }

    public override void StartText(string[] sDialogues)
    {
        MasterDialogueController.Instance.Activate.SetActive(false);
        base.StartText(sDialogues);
    }

    public override void AfterTextDisplay()
    {
        base.AfterTextDisplay();
        foreach (DialogueAchievement achievement in relatedAchievements)
        {
            achievement.validate();
        }
        foreach (Item item in itemAfterDialogue)
            InventoryManager.Instance.AddItem(item);
        MasterDialogueController.Instance.Activate.SetActive(true);
    }

    public void Subscribe()
    {
        if (!subscribed)
        {
            subscribed = true;
            MasterDialogueController.Instance.Activate.SetActive(!isPlaying);
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
                StopCoroutine(textDisplayCoroutine);
            isPlaying = false;
            MasterDialogueController.Instance.Text.text = "";
            MasterDialogueController.Instance.Unsubscribe(this);
        }
    }

    public void Interact()
    {
        if (!isPlaying)
            StartTextDisplay();
    }

    private void FixedUpdate()
    {
        if (Player.Instance.ValidateInteractDistance(transform.position))
        { 
            Subscribe();
        }
        else Unsubscribe();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, fTriggerDistance);
    }
#endif
}
