using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Boss Kill")]
public class BossKillAchievement : Achievement
{
    [SerializeField] private BossController bossPrefab;

    protected override bool validatePredicate()
    {
        return PlayerPrefs.GetInt(bossPrefab.GetHashCode().ToString(), 0) != 0;
    }
}
