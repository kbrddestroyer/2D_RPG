using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuyBoss : BossController
{
    [SerializeField, Range(0f, 10f)] private float postAttackImmunity;
    private bool inImmunity = true;

    public override float HP { 
        get => base.HP;
        set
        {
            if (!inImmunity) base.HP = value;
        }
    }

    public override void DamagePlayer(int animID)
    {
        base.DamagePlayer(animID);
        StartCoroutine(AttackDropState());
    }

    private IEnumerator AttackDropState()
    {
        inImmunity = false;
        float passed = 0f;
        while (passed < postAttackImmunity)
        {
            yield return new WaitForEndOfFrame();
            passed += Time.deltaTime;

            BossmodeGUI.Instance.Immunity = passed / postAttackImmunity;
        }

        inImmunity = true;
    }

}
