using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Base Achievement")]
public abstract class Achievement : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private string description;

    [SerializeField] private bool unlocked;

    public string Title { get => title; }
    public string Description { get => description; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    protected abstract bool validatePredicate();

    public bool validate()
    {
        if (validatePredicate() && !unlocked)
        {
            unlocked = true;
            return true;
        }
        return false;
    }
}
