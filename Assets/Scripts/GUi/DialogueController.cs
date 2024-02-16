using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private TMP_Text text;
    [SerializeField] private string[] sDialogues;
    [SerializeField, Range(0f, 1f)] private float fTextSpeed;
    [SerializeField, Range(0f, 10f)] private float fTriggerDistance;
    [SerializeField] private AudioSource audioSource;
    [Header("Gizmos")]
    [SerializeField] private Color gizmoColor = new Color(0, 0, 0, 1);

    private bool isPlaying = false;
    private bool skipCurrentText = false;

    private IEnumerator playText()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            foreach (string text in sDialogues)
            {
                this.text.text = "";
                foreach (char ch in text)
                {
                    if (skipCurrentText)
                    {
                        skipCurrentText = false;
                        break;
                    }
                    this.text.text += ch;
                    audioSource.Play();
                    yield return new WaitForSeconds(fTextSpeed);
                }
                this.text.text = text;
                while (!skipCurrentText) yield return null;
                skipCurrentText = false;
            }
            isPlaying = false;
            text.text = "";
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < fTriggerDistance)
        {
            if (isPlaying)
            {
                skipCurrentText = Input.GetKeyDown(KeyCode.Space);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                // Activate
                StartCoroutine(playText());
            }
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
