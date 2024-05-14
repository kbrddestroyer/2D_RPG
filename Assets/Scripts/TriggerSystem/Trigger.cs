using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider2D))]
public abstract class Trigger : MonoBehaviour
{
    [SerializeField] private new Collider2D collider;

    protected abstract void Action();
    protected abstract void Deactivate();

    protected void Start()
    {
        if (!collider.isTrigger)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Please, set collider to trigger mode for {name} trigger object");
#endif
            collider.isTrigger = true;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player)
            {
                if(!player.trigger) player.trigger = this;
                Action();
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player)
            {
                if (player.trigger == this) player.trigger = null;
                Deactivate();
            }
        }
    }
}
