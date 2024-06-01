using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider2D))]
public class BossController : MovingEnemy
{
    [Header("Boss Settings")]
    [SerializeField] private string bossname;
    [SerializeField] private AudioClip[] combat;
    [SerializeField] private int combatAnims;
    [SerializeField, Range(0f, 10f)] private float attackRange;
    [SerializeField, Range(0f, 10f)] private float damage;
    [SerializeField, Range(0f, 10f)] private float damageDelay;
    [Header("Boss Requirements")]
    [SerializeField] private Collider2D col;
    [Header("Boss Gizmo")]
    [SerializeField] private Color attackRangeColor = new Color(0, 0, 0, 1);

    private float passedTime = 0f;

    private new void Start()
    {
        base.Start();
        ActivateBossmode();
    }

    protected override void UpdateGUIElement(float value)
    {
        BossmodeGUI.Instance.HP = value;
    }

    public void ActivateBossmode()
    {
        BossmodeGUI.Instance.Toggle(true);
        BossmodeGUI.Instance.Name = bossname;
    }

    private void OnPostMortemDrop()
    {

    }

    public void DamagePlayer()
    {
        source.PlayOneShot(combat[Random.Range(0, combat.Length)]);
        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            player.HP -= damage;
        }
    }


    public override void OnDeath()
    {
        col.enabled = false;
        OnPostMortemDrop();
        BossmodeGUI.Instance.Toggle(false);
    }

    private int pickAttackType()
    {
        return Random.Range(0, combatAnims);
    }

    private void Attack()
    {
        int id = pickAttackType();
        passedTime = 0;

        animator.SetInteger("attackType", id);
        animator.SetTrigger("attack");
    }

    private new void Update()
    {
        base.Update();

        if (passedTime < damageDelay)
            passedTime += Time.deltaTime;

        else if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            Attack();
        }
    }

#if UNITY_EDITOR

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = attackRangeColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        base.OnDrawGizmosSelected();
    }

#endif
}
