using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AchievementGlobalList : ScriptableObject
{
    public List<Achievement> achievements = new List<Achievement>();
}
