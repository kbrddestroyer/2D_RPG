using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptedDialogueController : DialogueController
{
    [SerializeField] private UnityEvent onRead;

    public override void AfterTextDisplay()
    {
        onRead.Invoke();
    }

    public void KillPlayer()
    {
        Player player = FindObjectOfType<Player>();
        if (player)
        {
            player.HP = 0;
        }
    }
}
