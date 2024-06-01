using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuyBoss : BossController
{
    [Header("Big Guy Customs")]
    [Tooltip("Attack2 is linear and have different logic")]
    [SerializeField, Range(0f, 15f)] private float attack2_Range;
    [SerializeField, Range(0f, 15f)] private float attack2_Y_Dispersion;
    [SerializeField, Range(0f, 20f)] private float attack2_Damage;

    private Vector3 GetDirection()
    {
        return transform.right * ((spriteRenderer.flipX) ? -1 : 1);
    }

    private bool ValidatePlayerPositionForAttack2()
    {
        return
            Mathf.Abs(transform.position.x + GetDirection().x * attack2_Range / 2 - transform.position.x) <= attack2_Range &&
            Mathf.Abs(player.transform.position.y - transform.position.y) <= attack2_Y_Dispersion / 2;
    }

    public override void DamagePlayer(int damageType)
    {
        if (damageType == 1)
            base.DamagePlayer(damageType);
        else
        {
            if (ValidatePlayerPositionForAttack2())
            {
                player.HP -= attack2_Damage;
            }
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position + GetDirection() * attack2_Range / 2, new Vector3(attack2_Range, attack2_Y_Dispersion, 0));
    }
#endif
}
