using GameControllers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace GameControllers
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class SamuraiPlayer : Player
    {
        [Header("Samurai logic")]
        [SerializeField] private uint attackAnimsCount = 1;
        [SerializeField, Range(0f, 5f)] private float comboFallback;
        [SerializeField, Range(0f, 10f)] private float attack2_Damage;
        private float comboPassTime;
        private uint attackCount;

        protected override IEnumerator Blink()
        {
            yield return null;
        }

        protected override void Awake()
        {
            base.Awake();

            if (attackAnimsCount > combat.Length)
                Debug.LogError("Samurai logic requires unique sounds for each animation");
        }

        protected override AudioClip PickAttackSound()
        {
            return combat[attackCount % attackAnimsCount];
        }

        protected override void Attack()
        {
            animator.SetInteger("attackType", (int) (attackCount % attackAnimsCount));
            base.Attack();
            attackCount++;
            comboPassTime = 0;
        }

        protected override void UpdatePassedTimeGUI()
        {
            if (fPassedDelayTime < fDamageDelay)
            {
                fPassedDelayTime += Time.deltaTime;
            }

            if (comboPassTime < comboFallback)
            {
                comboPassTime += Time.deltaTime;
                HPGUIController.Instance.AttackProgression = 1 - comboPassTime / comboFallback;
                if (comboPassTime >= comboFallback)
                    attackCount = 0;
            }
        }
    }
}