using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDocumentController : MonoBehaviour
{
    [SerializeField] private ButtonController[] buttons;

    private void Start()
    {
        foreach (ButtonController controller in buttons)
            controller.Initialize();
    }
}
