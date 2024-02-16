using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingEnemy : EnemyBase, IDamagable
{
    public float HP { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }

    [SerializeField, Range(0f, 10f)] private float fReactionDistance;
    [SerializeField, Range(0f, 10f)] private float fDamage;
    [SerializeField, Range(0f, 1f)] private float fStopDistance;
    [SerializeField, Range(0f, 10f)] private float fSpeed;
    [Header("Gizmo settings")]
    [SerializeField] private Color gizmoColor = new Color(0, 0, 0, 1);

    private Stack<Vector3> currentWaypoints = new Stack<Vector3>();
    private Vector3 currentPoint;

    private void Awake()
    {
        currentPoint = transform.position;
    }

    public void Update()
    {
        currentWaypoints = getPath(player.transform.position);

        while (Vector3.Distance(transform.position, currentPoint) < fStopDistance)
        {
            currentPoint = currentWaypoints.Pop();
        }
        transform.position = Vector3.Lerp(transform.position, currentPoint, fSpeed * Time.deltaTime);

        Debug.DrawLine(transform.position, currentPoint, Color.green);        
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireSphere(transform.position, fReactionDistance);
        Gizmos.DrawWireSphere(transform.position, fStopDistance);
        
        base.OnDrawGizmosSelected();
    }
#endif
}
