using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dialogue : TalkerBase
{
    [SerializeField] protected string[] sDialogues;

    public void StartTextDisplay(string[] dialogue)
    {
        StartText(dialogue);
    }
    public void StartTextDisplay()
    {
        StartText(sDialogues);
    }

    public override void AfterTextDisplay() { }
}
