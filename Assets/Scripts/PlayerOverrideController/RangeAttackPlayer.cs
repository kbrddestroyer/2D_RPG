using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControllers
{
    public class RangeAttackPlayer : Player
    {
        [Header("Settings")]
        [SerializeField, Range(0f, 1f)] private float rotDisp;
        [SerializeField, Range(0f, 5f)] private float dashTime;
        [SerializeField, Range(0f, 5f)] private float dashSpeedMultiplier;
        [SerializeField] private Projectile projectile;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Collider2D col;

        private bool inDash = false;
        private float dashPassed = 0;

        public override float HP { 
            get => base.HP;
            set
            {
                if (!inDash)
                    base.HP = value;
            }
        }

        protected override IEnumerator Blink()
        {
            yield return null;
        }

        private void Dash()
        {
            if (dashPassed >= dashTime)
            {
                speedMultiplier = dashSpeedMultiplier;
                inDash = true;
                animator.SetTrigger("dash");
                col.excludeLayers = enemy;
                dashPassed = 0;
            }
        }

        public void ExitDash()
        {
            col.excludeLayers = 0;
            inDash = false;
            speedMultiplier = 1;
        }

        protected override void Attack()
        {
            if (!inDash)
                base.Attack();
        }

        public override void DealDamage(int attackID)
        {
            Projectile projectileInstance = Instantiate(projectile, spawnPoint.position, Quaternion.Euler(0, 0, Random.Range(-rotDisp, rotDisp)));
            projectileInstance.Damage = attacks[attackID].Damage;
            projectileInstance.Flip = spriteRenderer.flipX;
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) &&
                (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
                Dash();
            if (dashPassed < dashTime)
                dashPassed += Time.deltaTime;
            base.Update();
        }
    }
}
