using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSmoothener : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase baseTile;
    [SerializeField] private TileBase[] insideTiles;
    [SerializeField] private TileBase[] outsideTiles;

    private int sampleSize = 3;

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
        return y * sampleSize + x;
    }

    private Vector3Int getDirection(Vector3Int pivot, Vector3Int destination)
    {
        return new Vector3Int(pivot.x - destination.x, pivot.y - destination.y);
    }

    private void Smooth(Vector3Int pos)
    {
        List<Vector3Int> directions = new List<Vector3Int>();

        for (int i = 0; i < sampleSize; i++)
            for (int j = 0; j < sampleSize; j++)
            {
                Vector3Int destination = new Vector3Int(i - 1, j - 1);
                if (tilemap.GetTile(pos + destination) == baseTile)
                {
                    directions.Add(getDirection(pos, destination));
                }
            }

        Vector3Int completedDirection = Vector3Int.zero;
        foreach (Vector3Int direction in directions)
        {
            completedDirection += direction;
        }
        completedDirection.Clamp(new Vector3Int(-1, -1), new Vector3Int(1, 1));
        completedDirection *= (directions.Count > 1 ? -1 : 1);
        completedDirection += new Vector3Int(1, 1, 0);
        TileBase[] tiles = (directions.Count > 1 ? outsideTiles : insideTiles);
        Debug.Log(completedDirection);
        int id = getMonoFromDuoCoords(completedDirection.x, completedDirection.y);

        tilemap.SetTile(pos, tiles[id]);
    }

    public void SmoothGround()
    {
        BoundsInt bounds = tilemap.cellBounds;

        Vector2Int sampleStart = new Vector2Int(bounds.x, bounds.y);

        int z = bounds.z;

        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                Smooth(new Vector3Int(x, y));
            }
    }
#endif
}
