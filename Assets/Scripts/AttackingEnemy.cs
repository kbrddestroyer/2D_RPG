using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingEnemy : MovingEnemy, IDamagable
{
    [SerializeField, Range(0f, 10f)] private float fDamage;
    [SerializeField, Range(0f, 10f)] private float fAttackDelay;
    [SerializeField, Range(0f, 10f)] private float fAttackDistance;
    [SerializeField] private AudioClip attackSFX;
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

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = cAttackGizmoColor;
        Gizmos.DrawWireSphere(transform.position, fAttackDistance);
    
        base.OnDrawGizmosSelected();
    }
#endif
} 
