using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueController : MonoBehaviour
{
    [SerializeField] private string[] sDialogues;
    [SerializeField, Range(0f, 1f)] private float fTextSpeed;
    [SerializeField, Range(0f, 10f)] private float fTriggerDistance;
    [SerializeField] private AudioSource audioSource;
    [Header("Gizmos")]
    [SerializeField] private Color gizmoColor = new Color(0, 0, 0, 1);

    private MasterDialogueController dialogueController;

    public float TriggerDistance
    {
        get => fTriggerDistance;
    }

    private bool isPlaying = false;
    private bool skipCurrentText = false;

    private void Awake()
    {
        if (!dialogueController)
            dialogueController = GameObject.FindObjectOfType<MasterDialogueController>();
    }

    private IEnumerator playText()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            dialogueController.SpImage = true;
            foreach (string text in sDialogues)
            {
                dialogueController.Text.text = "";
                foreach (char ch in text)
                {
                    if (skipCurrentText)
                    {
                        skipCurrentText = false;
                        break;
                    }
                    dialogueController.Text.text += ch;
                    audioSource.Play();
                    yield return new WaitForSeconds(fTextSpeed);
                }
                dialogueController.Text.text = text;
                dialogueController.Skip.SetActive(!skipCurrentText);
                while (!skipCurrentText) yield return null;
                skipCurrentText = false;
                dialogueController.Skip.SetActive(skipCurrentText);
            }
            isPlaying = false;
            dialogueController.SpImage = false;
            dialogueController.Text.text = "";
        }
    }

    public void Activate(bool state)
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
                StartCoroutine(playText());
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

    private void Update()
    {
        skipCurrentText = Input.GetKeyDown(KeyCode.Space) && isPlaying;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, fTriggerDistance);
    }
#endif
}
