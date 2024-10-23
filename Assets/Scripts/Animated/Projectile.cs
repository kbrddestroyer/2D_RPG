using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float speed;
    [SerializeField, Range(0f, 50f)] private float lifetime;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool flip;
    private float fDamage;
    private bool damageDealt = false;

    public bool Flip { 
        set
        {
            flip = value;
            spriteRenderer.flipX = value;
        }
    }

    public float Damage { set => fDamage = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageDealt) return;

        if (collision.GetComponent<Projectile>())
            return;

        EnemyBase enemy = collision.GetComponent<EnemyBase>();
        if (enemy)
        {
            enemy.HP -= fDamage;
            lifetime = 0;
        }
        damageDealt = true;
        Destroy(this.gameObject, lifetime);
        enabled = false;
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        transform.position += transform.right * (flip ? -1 : 1) * speed * Time.deltaTime;
    }
}
