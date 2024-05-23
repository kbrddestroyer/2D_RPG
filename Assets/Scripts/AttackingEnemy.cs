using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class AttackingEnemy : MovingEnemy, IDamagable
{
    [Serializable]
    private struct ItemDrop
    {
        public Pickable item;
        public float chance;
    }

    [SerializeField, Range(0f, 10f)] private float fDamage;
    [SerializeField, Range(0f, 10f)] private float fAttackDelay;
    [SerializeField, Range(0f, 10f)] private float fAttackDistance;
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private new Collider2D collider;
    [Header("Drop")]
    [SerializeField] private ItemDrop[] drop;
    
    [Header("Gizmos")]
    [SerializeField] private Color cAttackGizmoColor = new Color(0f, 0f, 0f, 1f);

    private float fPassedTime = 0f;

    protected override void Update()
    {
        fPassedTime += Time.deltaTime;

        if (player != null && Vector2.Distance(transform.position, player.transform.position) < fAttackDistance)
        {
            Attack();
        }
        else base.Update();
    }

    private void Attack()
    {
        if (animator.GetBool("walking"))
        {
            animator.SetBool("walking", false);
        }
        if (fPassedTime >= fAttackDelay)
        {
            fPassedTime = 0;
            animator.SetTrigger("attack");
        }
    }

    private void DamagePlayer()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < fAttackDistance)
        { 
            player.HP -= fDamage;
            source.PlayOneShot(attackSFX);
        }
    }

    public override void OnDeath()
    {
        collider.enabled = false;

        foreach (ItemDrop item in drop)
        {
            float randomChance = UnityEngine.Random.Range(0, 100) / 100.0f;
            if (randomChance < item.chance)
            {
                Instantiate(item.item.gameObject, transform.position, transform.rotation);
            }
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = cAttackGizmoColor;
        Gizmos.DrawWireSphere(transform.position, fAttackDistance);
    
        base.OnDrawGizmosSelected();
    }
#endif
} 
