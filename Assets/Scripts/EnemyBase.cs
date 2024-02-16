using GameControllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private Tilemap wallsMap;
    [SerializeField] protected Player player;

    private class Waypoint {
        public Vector3Int point;
        public Waypoint parent;

        public Waypoint(Vector3Int point, Waypoint parent)
        {
            this.point = point;
            this.parent = parent;
        }
    }


    private Waypoint _getPath(Vector3 destination)
    {
        BoundsInt bounds = wallsMap.cellBounds;
        TileBase[] allTiles = wallsMap.GetTilesBlock(bounds);

        Vector3Int sourceTile = wallsMap.WorldToCell(transform.position);
        Vector3Int destinationTile = wallsMap.WorldToCell(destination);

        List<Vector3Int> closed = new List<Vector3Int>();
        Queue<Waypoint> open = new Queue<Waypoint>();
        
        open.Enqueue(new Waypoint(sourceTile, null));
        while (open.Count > 0)
        {
            Waypoint p = open.Dequeue();

            if (p.point == destinationTile)
            {
                return p;
            }
            if (closed.Contains(p.point))
            {
                continue;
            }

            for (int i = -1; i <= 1; i ++)
                for (int j = -1; j <= 1; j ++)
                {
                    if (i == 0 && j == 0) continue;

                    int x = p.point.x + i;
                    int y = p.point.y + j;

                    if (x < bounds.x || x > bounds.max.x || y < bounds.y || y > bounds.max.y)
                        continue;
                    Vector3Int pos = new Vector3Int(x, y, p.point.z);
                    if (wallsMap.GetTile(pos) == null)
                    {
                        open.Enqueue(new Waypoint(pos, p));
                    }
                }
            closed.Add(p.point);
        }

        return null;
    }

    protected Stack<Vector3> getPath(Vector3 destination)
    {
        Waypoint arr = _getPath(player.transform.position);
        if (arr == null) return null;
        Stack<Vector3> waypoints = new Stack<Vector3>();

        while (arr != null)
        {
            waypoints.Push(wallsMap.CellToLocal(arr.point) + wallsMap.cellSize * 0.5f);
            arr = arr.parent;
        }

        return waypoints;
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Waypoint arr = _getPath(player.transform.position);
        List<Vector3> waypoints = new List<Vector3>();

        while (arr != null) {
            waypoints.Add(wallsMap.CellToLocal(arr.point) + wallsMap.cellSize * 0.5f);
            arr = arr.parent;
        }

        ReadOnlySpan<Vector3> waypointsSpan = new ReadOnlySpan<Vector3>(waypoints.ToArray());
        Gizmos.DrawLineStrip(waypointsSpan, false);
    }
#endif
}
