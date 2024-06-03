using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField, Range(0f, 1f)] private float delay;

    [HideInInspector] public string title;

    private void OnEnable()
    {
        label.text = "";
        animator.SetTrigger("start");
    }

    IEnumerator typeText()
    {
        foreach (char ch in title)
        {
            label.text += ch;
            audioSource.PlayOneShot(audioSource.clip);
            yield return new WaitForSeconds(delay);
        }
    }

    public void TypeText()
    {
        StartCoroutine(typeText());
    }

    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
