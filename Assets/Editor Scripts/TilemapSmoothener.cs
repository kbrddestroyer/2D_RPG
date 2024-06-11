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

    private struct Sample
    {
        public Vector2Int sampleSize;
        public TileBase[,] sample; 

        public Sample(Vector2Int sampleSize)
        {
            this.sampleSize = sampleSize;
            sample = new TileBase[sampleSize.x, sampleSize.y];
        }

        public static void SmoothSample(ref TileBase[,] sample, Vector2Int sampleSize, TileBase baseTile)
        {
            Vector2Int pivot = new Vector2Int(sampleSize.x / 2, sampleSize.y / 2);

            if (sample[pivot.x, pivot.y] != baseTile)
                return;

            for (int x = 0; x < sampleSize.x; x++)
                for (int y = 0; y < sampleSize.y; y++)
                {
                    /* 
                     *      . . .
                     *      . * .
                     *      . . .
                     * 
                     */

                    Vector2Int direction = new Vector2Int(pivot.x - x, pivot.y - y);
                }
        }
    }

    private int getMonoFromDuoCoords(int x, int y)
    {

    }

    private TileBase getSmoothed(Vector3Int pos)
    {
        return null;
    }

    public void SmoothGround()
    {
        BoundsInt bounds = tilemap.cellBounds;

        Vector2Int sampleStart = new Vector2Int(bounds.x, bounds.y);

        int z = bounds.z;

        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                if (tilemap.GetTile(new Vector3Int(x, y, z)) == baseTile)
                {

                }
            }
    }
#endif
}
