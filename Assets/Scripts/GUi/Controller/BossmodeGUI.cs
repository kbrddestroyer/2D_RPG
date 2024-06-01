using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossmodeGUI : MonoBehaviour, ISingleton
{
    private static BossmodeGUI instance;
    public static BossmodeGUI Instance { get => instance; }

    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text bossName;
    [SerializeField] private Slider bossHP;

    public string Name { set => bossName.text = value; }
    public float HP
    {
        set
        {
            bossHP.value = Mathf.Clamp(value, 0f, 1f);
        }
    }

    public void Toggle(bool state)
    {
        canvas.SetActive(state);
    }

    public void Awake()
    {
        instance = this;
    }
}
