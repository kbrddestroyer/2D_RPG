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

    public override void Activate(bool state)
    {
        if (state)
        {
            Subscribe();
            MasterDialogueController.Instance.Activate.SetActive(!isPlaying);
            if (isPlaying)
            {
                if (!MasterDialogueController.Instance.Skip.activeInHierarchy) MasterDialogueController.Instance.Skip.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartTextDisplay();
            }
        }
        else
        {
            Unsubscribe();
            if (!MasterDialogueController.Instance.Enabled)
            {
                if (textDisplayCoroutine != null)
                    StopCoroutine(textDisplayCoroutine);
                isPlaying = false;
            }
        }
    }

    private void StartTextDisplay()
    {
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
