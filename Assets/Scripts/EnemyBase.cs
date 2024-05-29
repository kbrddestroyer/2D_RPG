using GameControllers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour, IDamagable
{
    [SerializeField] private Tilemap wallsMap;
    [SerializeField] protected Player player;
    [SerializeField] protected Animator animator;
    [SerializeField, Range(0f, 10f)] private float fMaxHP;
    [SerializeField, Range(0f, 10f)] private float fCorpseLifetime;
    [Header("GUI")]
    [SerializeField] private Slider hpSlider;

    private float fHP;

    public float HP { 
        get => fHP; 
        set
        {
            if (value < fHP && value > 0)
                Damage();
            fHP = value;
            hpSlider.value = Mathf.Clamp(value / fMaxHP, 0f, 1f);
            if (fHP <= 0f)
                enabled = false;
        }
    }

    protected virtual void Awake()
    {
        HP = fMaxHP;
    }

    private void OnDisable()
    {
        animator.SetTrigger("death");
        hpSlider.gameObject.SetActive(false);
        OnDeath();
        Destroy(this.gameObject, fCorpseLifetime);
    }

    private void Damage()
    {
        animator.SetTrigger("damage");
    }

    private class Waypoint {
        public Vector3Int point;
        public Waypoint parent;
        public float weight;
        public Waypoint(Vector3Int point, Waypoint parent, Tilemap wallsMap, Player player)
        {
            this.point = point;
            this.parent = parent;
            this.weight = Vector2.Distance(wallsMap.LocalToWorld(point), player.transform.position);
        }
    }

    private class PriorityQueue<ob> : Dictionary<float, ob> { 
        public ob Minimum()
        {
            if (this.Count == 0)
                return default(ob);

            float minimum = this.Keys.AsReadOnlyList()[0];
            foreach (float p in this.Keys.AsReadOnlyList())
            {
                if (p < minimum)
                    minimum = p;
            }

            ob obj = this[minimum];
            this.Remove(minimum);

            return obj;
        }
    }


    private Waypoint getPathFromDestinationCoordinates(Vector3 destination)
    {
        BoundsInt bounds = wallsMap.cellBounds;
        TileBase[] allTiles = wallsMap.GetTilesBlock(bounds);

        Vector3Int sourceTile = wallsMap.WorldToCell(transform.position);
        Vector3Int destinationTile = wallsMap.WorldToCell(destination);

        List<Vector3Int> closed = new List<Vector3Int>();
        PriorityQueue<Waypoint> open = new PriorityQueue<Waypoint>();

        Waypoint start = new Waypoint(sourceTile, null, wallsMap, player);

        open[start.weight] = start;

        while (open.Count > 0)
        {
            Waypoint p = open.Minimum();

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
                        Waypoint point = new Waypoint(pos, p, wallsMap, player);
                        open[point.weight] = point;
                    }
                }
            closed.Add(p.point);
        }

        return null;
    }

    protected Stack<Vector3> getPath(Vector3 destination)
    {
        Waypoint arr = getPathFromDestinationCoordinates(player.transform.position);
        if (arr == null) return null;
        Stack<Vector3> waypoints = new Stack<Vector3>();

        while (arr != null)
        {
            waypoints.Push(wallsMap.CellToLocal(arr.point) + wallsMap.cellSize * 0.5f);
            arr = arr.parent;
        }

        return waypoints;
    }

    public abstract void OnDeath();

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Waypoint arr = getPathFromDestinationCoordinates(player.transform.position);
        List<Vector3> waypoints = new List<Vector3>();

        while (arr != null) {
            waypoints.Add(wallsMap.CellToLocal(arr.point) + wallsMap.cellSize * 0.5f);
            arr = arr.parent;
        }

        ReadOnlySpan<Vector3> waypointsSpan = new ReadOnlySpan<Vector3>(waypoints.ToArray());
        Gizmos.DrawLineStrip(waypointsSpan, false);
    }

    protected virtual void OnValidate()
    {
        if (!wallsMap)
        {
            wallsMap = GameObject.Find("Walls").GetComponent<Tilemap>();
            if (!wallsMap)
            {
                Debug.LogError("Can't find tilemap on scene. Please, make sure you have walls tilemap");
            }
        }
    }
#endif
}
