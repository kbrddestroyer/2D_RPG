using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoneGolem : AttackingEnemy
{
    [Header("Stone Golem Individual")]
    [SerializeField, Range(0f, 10f)] private float attack2_Damage;
    [SerializeField, Range(0f, 10f)] private float attack2_Distance;
    [SerializeField, Range(0f, 10f)] private float attack2_Y_Dispersion;

    private Vector3 GetOffset()
    {
        return new Vector3(
            attack2_Distance / 2 * (spriteRenderer.flipX ? -1 : 1), 
            0,
            0
            );
    }

    private bool ValidatePlayerPosition()
    {
        return
            (
                Mathf.Abs(player.transform.position.x - transform.position.x) <= attack2_Distance &&
                Mathf.Abs(player.transform.position.y - transform.position.y) <= attack2_Y_Dispersion / 2
            );
    }

    public void OnDamageFromAnimator(int id)
    {
        if (id == 1)
            DamagePlayer();
        else
        {
            source.PlayOneShot(attackSFX);

            if (ValidatePlayerPosition())
            {
                player.HP -= attack2_Distance;
            }
        }
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.DrawWireCube(transform.position + GetOffset(), new Vector3(attack2_Distance, attack2_Y_Dispersion / 2, 0));
    }
#endif
}
