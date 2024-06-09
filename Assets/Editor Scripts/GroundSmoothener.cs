using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TilemapSmoothener))]
public class GroundSmoothener : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TilemapSmoothener tilemap = (TilemapSmoothener) target;
    
        if (GUILayout.Button("Smooth ground"))
        {
            tilemap.SmoothGround();
        }
    }
}
#endif
