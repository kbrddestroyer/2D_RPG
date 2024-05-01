using GameControllers;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class MovingEnemy : EnemyBase, IDamagable
{
    [SerializeField, Range(0f, 10f)] private float fReactionDistance;
    [SerializeField, Range(0f, 1f)] private float fRerouteDistance;
    [SerializeField, Range(0f, 10f)] private float fStopDistance;
    [SerializeField, Range(0f, 10f)] private float fSpeed;
    [SerializeField] private AudioClip stepSFX;
    [SerializeField] protected AudioSource source;
    [Header("Gizmo settings")]
    [SerializeField] private Color gizmoColor = new Color(0, 0, 0, 1);

    private Stack<Vector3> currentWaypoints = new Stack<Vector3>();
    private Vector3 currentPoint;

    protected override void Awake()
    {
        currentPoint = transform.position;
        base.Awake();
    }

    protected virtual void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < fReactionDistance && distance > fStopDistance)
        {
            if (!animator.GetBool("walking"))
            {
                animator.SetBool("walking", true);
            }
            if (currentWaypoints.Count == 0)
                currentWaypoints = getPath(player.transform.position);

            if (Vector2.Distance(transform.position, currentPoint) < fRerouteDistance)
            {
                currentPoint = currentWaypoints.Pop();
            }
            transform.position = Vector3.Lerp(transform.position, currentPoint, fSpeed * Time.deltaTime);

            Debug.DrawLine(transform.position, currentPoint, Color.green);
        }
        else if (animator.GetBool("walking"))
        {
            animator.SetBool("walking", false);
        }
    }

    public void PlayStepSound()
    {
        source.PlayOneShot(stepSFX);
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireSphere(transform.position, fReactionDistance);
        Gizmos.DrawWireSphere(transform.position, fStopDistance);
        Gizmos.DrawWireSphere(transform.position, fRerouteDistance);
        base.OnDrawGizmosSelected();
    }

    protected override void OnValidate()
    {
        if (!player)
        {
            player = FindObjectOfType<Player>();
        }
        base.OnValidate();
    }
#endif
}
