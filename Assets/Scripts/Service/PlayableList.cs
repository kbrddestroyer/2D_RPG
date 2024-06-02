using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Playable Global List")]
public class PlayableList : ScriptableObject
{
    public Player[] playable;
}
