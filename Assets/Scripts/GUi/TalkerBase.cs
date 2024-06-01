using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TalkerBase : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] protected float fTextSpeed;
    [SerializeField] protected AudioSource audioSource;

    protected bool isPlaying = false;
    protected bool skipCurrentText = false;
    protected Coroutine textDisplayCoroutine = null;

    public void StartText(string[] sDialogues)
    {
        textDisplayCoroutine = StartCoroutine(playText(sDialogues));
    }

    protected IEnumerator playText(string[] sDialogues)
    {
        if (!isPlaying)
        {
            isPlaying = true;
            MasterDialogueController.Instance.SpImage = true;
            foreach (string text in sDialogues)
            {
                MasterDialogueController.Instance.Text.text = "";
                foreach (char ch in text)
                {
                    if (skipCurrentText)
                    {
                        skipCurrentText = false;
                        break;
                    }
                    MasterDialogueController.Instance.Text.text += ch;
                    audioSource.Play();
                    yield return new WaitForSeconds(fTextSpeed);
                }
                MasterDialogueController.Instance.Text.text = text;
                MasterDialogueController.Instance.Skip.SetActive(!skipCurrentText);
                while (!skipCurrentText) yield return null;
                skipCurrentText = false;
                MasterDialogueController.Instance.Skip.SetActive(skipCurrentText);
            }
            isPlaying = false;
            MasterDialogueController.Instance.SpImage = false;
            MasterDialogueController.Instance.Text.text = "";

            AfterTextDisplay();
        }
    }

    public abstract void AfterTextDisplay();

    public abstract void Activate(bool state);

    protected virtual void Update()
    {
        skipCurrentText = Input.GetKeyDown(KeyCode.Space) && isPlaying;
    }
}
