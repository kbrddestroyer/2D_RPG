using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBase : TalkerBase, IMasterDialogue
{
    [SerializeField] private QuestItem item;
    [SerializeField] private string[] startQuestDialogue;
    [SerializeField] private string[] inProgressQuestDialogue;
    [SerializeField] private string[] questCompletedText;
    [SerializeField] private string[] questInactive;

    [Header("Gizmos")]
    [SerializeField] protected Color gizmoColor = new Color(0, 0, 0, 1);

    private bool subscribed = false;

    private void StartTextDisplay()
    {
        MasterDialogueController.Instance.Activate.SetActive(false);
        if (item.questStarted)
        {
            if (item.questCompleted)
            {
                OnQuestInactive();
            }
            else if (item.questValidationPredicate())
            {
                StopQuest();
            }
            else
                OnQuestInProgress();
        }
        else
            StartQuest();
    }

    public virtual void StartQuest()
    {
        StartText(startQuestDialogue);
        item.questStarted = true;
    }

    public virtual void OnQuestInProgress()
    {
        StartText(inProgressQuestDialogue);
    }

    public virtual void StopQuest()
    {
        StartText(questCompletedText);
        item.questCompleted = true;
        InventoryManager.Instance.AddItem(item.Reward);
    }

    public virtual void OnQuestInactive()
    {
        StartText(questInactive);
    }

    public override void AfterTextDisplay()
    {
        MasterDialogueController.Instance.Activate.SetActive(true);
        if (item.questStarted)
        {
            if (item.questCompleted)
                InventoryManager.Instance.DeactivateQuest(item);
            else
                InventoryManager.Instance.StartQuest(item);
        }
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
}
