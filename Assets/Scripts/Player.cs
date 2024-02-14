using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace GameControllers 
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IDamagable
    {
        [Header("Base settings")]
        [SerializeField, Range(0f, 10f)] private float fSpeed;
        [SerializeField, Range(0f, 10f)] private float fDamage;
        [SerializeField, Range(0f, 10f)] private float fDamageDistance;
        [SerializeField, Range(0f, 10f)] private float fMaxHP;
        [SerializeField] private LayerMask enemy;

        [Header("Required Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Gizmos")]
        [SerializeField] private Color gizmoColor = new Color(0f, 0f, 0f, 1f);

        private float fHP;

        public float HP
        {
            get => fHP;
            set
            {
                fHP = value;
                if (value <= 0)
                {
                    this.enabled = false;
                }
            }
        }

        private void OnEnable()
        {
            // Nothing here by now
        }

        private void OnDisable()
        {
            OnDeath();
            Destroy(this.gameObject, 1f);
        }

        public void OnDeath()
        {
            animator.SetTrigger("death");
        }

        private void Attack()
        {
            animator.SetTrigger("attack");

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fDamageDistance, enemy);

            foreach (Collider2D collider in colliders)
            {
                IDamagable damagable;
                if ((damagable = collider.GetComponent<IDamagable>()) != null) {
                    damagable.HP -= fDamage;
                }
            }
        }

        private void Update()
        {
            Vector2 inputSpeed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * fSpeed;  // Direction vector
            spriteRenderer.flipX = (inputSpeed.x == 0) ? spriteRenderer.flipX : inputSpeed.x < 0;

            animator.SetBool("walking", inputSpeed.magnitude > 0);

            transform.Translate(inputSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
        }

#if UNITY_EDITOR || UNITY_EDITOR_64
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, fDamageDistance);
        }
#endif
    }
}
