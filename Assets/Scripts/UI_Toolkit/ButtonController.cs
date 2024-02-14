using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[Serializable]
public class ButtonController {
    [Header("UI Setup")]
    [SerializeField] private string id;
    [SerializeField] private UIDocument document;
    [Header("Action")]
    [SerializeField] private UnityEvent action = new UnityEvent();
    [SerializeField] private UnityEvent hover = new UnityEvent();

    public void Initialize()
    {
        Button btn = document.rootVisualElement.Q<Button>(id);
        btn.RegisterCallback<ClickEvent>((ClickEvent e) => { InvokeAction(action); });
        btn.RegisterCallback<MouseOverEvent>((MouseOverEvent e) => { InvokeAction(hover); });
    }

    private void InvokeAction(UnityEvent action)
    {
        action.Invoke();
    }
}
