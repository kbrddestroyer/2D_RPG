using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class AttackingEnemy : MovingEnemy, IDamagable
{
    [System.Serializable]
    private struct ItemDrop
    {
        public Pickable item;
        public float chance;
    }

    [SerializeField] protected Attack[] attacks;
    [SerializeField, Range(0f, 10f)] private float fAttackDelay;
    [SerializeField] private new Collider2D collider;
    [Header("Drop")]
    [SerializeField] private ItemDrop[] drop;
    [Header("Gizmos")]
    [SerializeField] private Color cAttackGizmoColor = new Color(0f, 0f, 0f, 1f);

    private float fPassedTime = 0f;
    private int pickedAttackID = 0;

    protected override void Update()
    {
        fPassedTime += Time.deltaTime;
        if (summoned && fPassedTime > fAttackDelay && player != null && attacks[pickedAttackID].validatePredicate(transform.position, player.transform.position, spriteRenderer.flipX))
        {
            Attack();
        }
        else base.Update();
    }

    protected virtual void Attack()
    {
        if (animator.GetBool("walking"))
        {
            animator.SetBool("walking", false);
        }

        fPassedTime = 0;
        animator.SetInteger("attackType", Random.Range(0, attacks.Length));
        animator.SetTrigger("attack");
    }

    public virtual void DamagePlayer(int animID)
    {
        if (animID >= attacks.Length)
            return;

        if (attacks[animID].validatePredicate(transform.position, player.transform.position, spriteRenderer.flipX))
        { 
            player.HP -= attacks[animID].Damage;
        }
        source.PlayOneShot(attacks[animID].SFX);

        pickedAttackID = Random.Range(0, attacks.Length);
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
        foreach (Attack attack in attacks)
        {
            attack.DrawGizmo(transform.position);
        }
    
        base.OnDrawGizmosSelected();
    }
#endif
} 
