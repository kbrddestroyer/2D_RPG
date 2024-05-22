using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBase : TalkerBase, IMasterDialogue
{
    [SerializeField] private QuestItemObtainItem item;
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
            {
                InventoryManager.Instance.DeactivateQuest(item);
                InventoryManager.Instance.RemoveItem(item.ItemToObtain.ItemSetting);
            }
            else
                InventoryManager.Instance.StartQuest(item);
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
}
