using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GUIControllers
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Header("Base Settings")]
        [SerializeField, Range(0f, 10f)] private float fSpeed;
        [SerializeField, Range(0f, 10f)] private float fMinRange;
        [SerializeField, Range(0f, 1f)] private float fStopRange;
        [Header("Required")]
        [SerializeField] private Transform player;
        [Header("Gizmos")]
        [SerializeField] private Color gizmoColor = new Color(0f, 0f, 0f, 1f);

        private bool isMoving = false;

        private void Update()
        {
            if (Vector2.Distance(player.position, transform.position) >= fMinRange || isMoving)
            {
                // Change camera position
                transform.position = Vector3.Lerp(transform.position, player.position + Vector3.back * 10, fSpeed * Time.deltaTime);
                isMoving = (Vector2.Distance(player.position, transform.position) > fStopRange);
            }
        }

#if UNITY_EDITOR || UNITY_EDITOR_64
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, fMinRange);
            Gizmos.DrawWireSphere(transform.position, fStopRange);
            Handles.Label(transform.position + Vector3.up * 2f, $"Reaction distance is {fMinRange}");
        }
#endif
    }
}