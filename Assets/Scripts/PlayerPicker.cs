using GameControllers;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEngine;

public class PlayerPicker : MonoBehaviour, IMasterDialogue
{
    [SerializeField] private PlayableList globalPlayables;
    [SerializeField] private int playerID;

    private bool subscribed = false;

    public void Interact()
    {
        PlayerPrefs.SetInt("playable", playerID);
        Vector3 position = Player.Instance.transform.position;
        Destroy(Player.Instance.gameObject);
        Instantiate(globalPlayables.playable[playerID], position, Quaternion.identity);

        PlayerPrefs.Save();
    }

    public void Subscribe()
    {
        if (!subscribed)
        {
            subscribed = true;
            MasterDialogueController.Instance.Activate.SetActive(true);
            MasterDialogueController.Instance.Subscribe(this);
        }
    }

    public void Unsubscribe()
    {
        if (subscribed)
        {
            subscribed = false;
            MasterDialogueController.Instance.Activate.SetActive(false);
            MasterDialogueController.Instance.Unsubscribe(this);
        }
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
