using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableTrigger : Trigger
{
    [SerializeField] private UnityEvent actionEvent;
    [SerializeField] private UnityEvent deactivationEvent;

    protected override void Action()
    {
        actionEvent.Invoke();
    }

    protected override void Deactivate()
    {
        deactivationEvent.Invoke();
    }
}
