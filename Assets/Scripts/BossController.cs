using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossController : MovingEnemy
{
    [Header("Boss Settings")]
    [SerializeField] private string bossname;
    [SerializeField] protected AudioClip[] combat;
    [SerializeField] protected int combatAnims;
    [SerializeField, Range(0f, 10f)] private float attackRange;
    [SerializeField, Range(0f, 10f)] private float damage;
    [SerializeField, Range(0f, 10f)] private float damageDelay;
    [SerializeField, Range(0f, 10f)] private float postAttackDelay;
    [Header("Boss Requirements")]
    [SerializeField] private Collider2D col;
    [SerializeField] private Pickable[] postMortemDrop;
    [Header("Boss Gizmo")]
    [SerializeField] private Color attackRangeColor = new Color(0, 0, 0, 1);

    protected float passedTime = 0f;
    protected bool inAttack = false;

    public override float HP 
    { 
        get => base.HP;
        set 
        { 
            if (!inAttack) base.HP = value; 
        }
    }

    private IEnumerator AttackDropState()
    {
        inAttack = true; 
        yield return new WaitForSeconds(postAttackDelay);
        inAttack = false;
    }

    private new void Start()
    {
        base.Start();
        ActivateBossmode();
    }

    protected override void UpdateGUIElement(float value)
    {
        BossmodeGUI.Instance.HP = value;
    }

    public virtual void ActivateBossmode()
    {
        BossmodeGUI.Instance.Toggle(true);
        BossmodeGUI.Instance.Name = bossname;
    }

    protected virtual void OnPostMortemDrop()
    {
        foreach (Pickable pickable in postMortemDrop)
        {
            Instantiate(pickable, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
        }
        Destroy(gameObject, postAttackDelay);
    }

    public void PlayerDamageFromAnimator(int damageType)
    {
        DamagePlayer(damageType);
        source.PlayOneShot(combat[Random.Range(0, combat.Length)]);
    }

    public virtual void DamagePlayer(int damageType)
    {
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

    protected virtual void Attack()
    {
        StartCoroutine(AttackDropState());
        int id = pickAttackType();
        passedTime = 0;

        animator.SetInteger("attackType", id);
        animator.SetTrigger("attack");
    }

    protected override void Update()
    {
        if (passedTime < damageDelay)
            passedTime += Time.deltaTime;
        else if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            Attack();
        }

        if (!inAttack)
            base.Update();
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
