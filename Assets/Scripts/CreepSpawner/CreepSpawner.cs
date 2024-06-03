using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0f, 10f)] private float range;
    [SerializeField, Range(0f, 100f)] private float activationDistance;
    [SerializeField, Range(0f, 10f)] private float delay;
    [SerializeField, Range(0, 10)] private uint creepCount;
    [Header("Requirements")]
    [SerializeField] private EnemyBase creep;
    [Header("Gizmo")]
    [SerializeField] private Color gizmoColor = new Color(0, 0, 0, 1);

    private float passedTime = 0f;

    private Vector3 RandomAspect()
    {
        return new Vector3(
            Random.Range(-range, range),
            Random.Range(-range, range), 
            0           
            );
    }

    public void Update()
    {
        if (passedTime < delay)
            passedTime += Time.deltaTime;
        else
        {
            if (Vector3.Distance(Player.Instance.transform.position, transform.position) > activationDistance)
                return;
            
            passedTime = 0f;
            for (uint i = 0; i < creepCount; i++) 
                Instantiate(creep, transform.position + RandomAspect(), Quaternion.identity);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireSphere(transform.position, activationDistance);
    }
#endif
}
