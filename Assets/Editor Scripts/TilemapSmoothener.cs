using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSmoothener : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase baseTile;
    [SerializeField] private TileBase[] insideTiles;
    [SerializeField] private TileBase[] outsideTiles;

    public void SmoothGround()
    {
        BoundsInt bounds = tilemap.cellBounds;

        Vector2Int sampleStart = new Vector2Int(bounds.x, bounds.y);
        Vector2Int sampleSize = new Vector2Int(2, 2);


    }
#endif
}
