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
        [SerializeField] private LayerMask pickables;

        [Header("Required Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource audio;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Audio")]
        [SerializeField] private AudioClip stepSFX;
        [SerializeField] private AudioClip damagedSFX;

        [Header("Gizmos")]
        [SerializeField] private Color gizmoColor = new Color(0f, 0f, 0f, 1f);

        private float fHP;    

        public float HP
        {
            get => fHP;
            set
            {
                if (value < fHP)
                {
                    Debug.Log("Player's damaged");
                    animator.SetTrigger("damage");
                    audio.PlayOneShot(damagedSFX);
                }
                fHP = value;
                if (value <= 0)
                {
                    this.enabled = false;
                }
            }
        }

        private IEnumerator Blink()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(5f);
                animator.SetTrigger("blink");
            }
        }


        private void Awake()
        {
            StartCoroutine(Blink());
        }

        private void OnEnable()
        {
            fHP = fMaxHP;
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

        private IPickable GetClosestPickableItem() 
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fDamageDistance, pickables);

            IPickable closest = null;

            foreach (Collider2D collider in colliders)
            {
                IPickable pickable;
                if ((pickable = collider.GetComponent<IPickable>()) != null)
                {
                    closest = pickable;
                }
            }

            List<IPickable> lPickables = new List<IPickable>(FindObjectsOfType<Pickable>());
            lPickables.RemoveAll((a) => a == closest);
            lPickables.RemoveAll((a) => !a.hint);

            foreach (IPickable pickable in lPickables)
            {
                pickable.hint = false;
            }

            return closest;
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
            IPickable closest;
            if ((closest = GetClosestPickableItem()) != null)
            {
                closest.hint = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    closest.Pickup();
                }
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
