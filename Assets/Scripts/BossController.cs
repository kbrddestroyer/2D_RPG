using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossController : AttackingEnemy
{
    [Header("Boss Settings")]
    [SerializeField] private string bossname;
    [SerializeField, Range(0f, 10f)] private float postAttackDelay;
    [Header("Boss Requirements")]
    [SerializeField] private Collider2D col;

    protected float passedTime = 0f;
    public bool inAttack = false;

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
        float passed = 0f;
        while (passed < postAttackDelay)
        {
            yield return new WaitForEndOfFrame();
            passed += Time.deltaTime;

            BossmodeGUI.Instance.Immunity = passed / postAttackDelay;
        }

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

    protected override void Attack()
    {
        if (inAttack) return;
        StopAllCoroutines();
        StartCoroutine(AttackDropState());
        base.Attack();
    }

    protected override void Update()
    {
        base.Update();
    }
}
