using GameControllers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class Attack
{
    [SerializeField, Range(0f, 10f)] private float fDamage;
    [SerializeField, Range(0f, 10f)] private float fDamageDistance;
    [SerializeField, Range(0f, 10f)] private float fYDispersion;
    [SerializeField] private AttackType type;
    [SerializeField] private AudioClip sfx;
    
    public float Damage { get => fDamage; }
    public float Dispersion { get => fYDispersion; }
    public float Distance { get => fDamageDistance; }
    public AudioClip SFX { get => sfx; }

    public bool validatePredicate(Vector3 position, float correction, Vector3 playerPosition, bool flipped)
    {
        switch (type)
        {
            case AttackType.ATTACK_FORWARD:
                return (
                    Mathf.Abs(playerPosition.x - position.x) - correction / 2 <= fDamageDistance &&
                    Mathf.Abs(playerPosition.y - position.y) - correction / 2 <= fYDispersion / 2
                );
            case AttackType.ATTACK_RANGE:
                return Vector3.Distance(position, playerPosition) <= fDamageDistance + correction;
            case AttackType.ATTACK_PROJECTILE:
                return false;
            default:
                Debug.LogWarning($"Not implemented {type} in getRootPosition");
                return false;
        }
    }

#if UNITY_EDITOR
    private Vector3 getRootPosition(bool flipped)
    {
        switch (type)
        {
            case AttackType.ATTACK_FORWARD:
                return new Vector3(
                    fDamageDistance / 2 * (flipped ? -1 : 1),
                    0,
                    0
                    );
            case AttackType.ATTACK_RANGE:
                return Vector3.zero;
            default:
                Debug.LogWarning($"Not implemented {type} in getRootPosition");
                return Vector3.zero;
        }
    }

    public void DrawGizmo(Vector3 position, bool flipped)
    {
        switch (type)
        {
            case AttackType.ATTACK_FORWARD:
                Gizmos.DrawWireCube(position + getRootPosition(flipped), new Vector3(fDamageDistance, fYDispersion / 2, 0));
                break;
            case AttackType.ATTACK_RANGE:
                Gizmos.DrawWireSphere(position, fDamageDistance);
                break;
            default:
                Debug.LogWarning($"Not implemented {type} in getRootPosition");
                return;
        }
    }
#endif
}
