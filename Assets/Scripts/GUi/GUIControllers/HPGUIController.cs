using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGUIController : MonoBehaviour
{
    private static HPGUIController instance;
    public static HPGUIController Instance { get => instance; }

    [SerializeField] private Slider slider;

    public float HP
    {
        set => slider.value = Mathf.Clamp(value, 0f, 1f);
    }

    private void Awake()
    {
        instance = this;
    }
}
