using GUIControllers;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace GameControllers 
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IDamagable
    {
        [Header("Base settings")]
        [SerializeField, Range(0f, 25f)] private float fSpeed;
        [SerializeField, Range(0f, 10f)] private float fDamage;
        [SerializeField, Range(0f, 1f)]  protected float fDamageDelay;
        [SerializeField, Range(0f, 10f)] private float fDamageDistance;
        [SerializeField, Range(0f, 10f)] private float fTriggerDistance;
        [SerializeField, Range(0f, 10f)] private float fMaxHP;
        [SerializeField] private LayerMask enemy;
        [SerializeField] private LayerMask pickables;

        [Header("Required Components")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected new AudioSource audio;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [Header("Audio")]
        [SerializeField] private AudioClip stepSFX;
        [SerializeField] private AudioClip damagedSFX;
        [SerializeField] protected AudioClip[] combat;

        [Header("Gizmos")]
        [SerializeField] private Color gizmoColor = new Color(0f, 0f, 0f, 1f);
        [SerializeField] private Color triggerGizmoColor = new Color(0f, 0f, 0f, 1f);

        public Trigger trigger = null;

        private float fHP;
        protected float fPassedDelayTime = 0.0f;
        private List<TalkerBase> lDialogueController;
        protected MasterDialogueController masterController;

        public float HP
        {
            get => fHP;
            set
            {
                if (value < fHP)
                {
                    if (fHP > 0)
                    {
                        animator.SetTrigger("damage");
                        CameraController.Instance.Damage();
                        audio.PlayOneShot(damagedSFX);
                    }
                }
                fHP = value;
                HPGUIController.Instance.HP = value / fMaxHP;
                if (value <= 0)
                {
                    this.enabled = false;
                }
            }
        }

        protected virtual IEnumerator Blink()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(5f);
                animator.SetTrigger("blink");
            }
        }


        protected virtual void Awake()
        {
            StartCoroutine(Blink());
            lDialogueController = FindObjectsOfType<TalkerBase>().ToList<TalkerBase>();

            if (!masterController)
                masterController = GameObject.FindObjectOfType<MasterDialogueController>();
        }

        protected virtual void OnEnable()
        {
            fHP = fMaxHP;
        }

        protected virtual void OnDisable()
        {
            OnDeath();
            Destroy(this.gameObject, 1f);
        }

        public virtual void OnDeath()
        {
            animator.SetTrigger("death");
        }

        protected virtual void OnDestroy()
        {
            if (HP <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void DealDamage()
        {
            animator.ResetTrigger("attack");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fDamageDistance, enemy);

            foreach (Collider2D collider in colliders)
            {
                IDamagable damagable;
                if ((damagable = collider.GetComponent<IDamagable>()) != null)
                {
                    damagable.HP -= fDamage;
                }
            }
        }

        protected virtual AudioClip PickAttackSound()
        {
            return combat[Random.Range(0, combat.Length)];
        }

        protected virtual void Attack()
        {
            if (fPassedDelayTime < fDamageDelay)
                return;
            fPassedDelayTime = 0.0f;
            animator.SetTrigger("attack");
            audio.PlayOneShot(PickAttackSound());
        }

        protected IPickable GetClosestPickableItem() 
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fTriggerDistance, pickables);

            IPickable closest = null;

            foreach (Collider2D collider in colliders)
            {
                IPickable pickable;
                if ((pickable = collider.GetComponent<IPickable>()) != null)
                {
                    closest = pickable;
                }
            }
            return closest;
        }

        protected TalkerBase GetClosestDialogueController()
        {
            TalkerBase closest = null;

            foreach (TalkerBase controller in lDialogueController)
            {
                float distance = Vector2.Distance(transform.position, controller.transform.position);

                if (distance < fTriggerDistance)
                {
                    if (closest == null || distance < Vector2.Distance(transform.position, closest.transform.position))
                        closest = controller;
                }
            }
            return closest;
        }

        public void Step()
        {
            audio.PlayOneShot(stepSFX);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            IMasterDialogue controller = collision.GetComponent<IMasterDialogue>();

            if (controller != null)
                controller.Subscribe();
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            IMasterDialogue controller = collision.GetComponent<IMasterDialogue>();

            if (controller != null)
                controller.Unsubscribe();
        }

        protected virtual void UpdatePassedTimeGUI()
        {
            if (fPassedDelayTime < fDamageDelay)
            {
                fPassedDelayTime += Time.deltaTime;

                HPGUIController.Instance.AttackProgression = fPassedDelayTime / fDamageDelay;
            }
        }

        protected virtual void Update()
        {
            Vector2 inputSpeed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * fSpeed;  // Direction vector
            spriteRenderer.flipX = (inputSpeed.x == 0) ? spriteRenderer.flipX : inputSpeed.x < 0;

            UpdatePassedTimeGUI();

            animator.SetBool("walking", inputSpeed.magnitude > 0);

            transform.Translate(inputSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }

            IPickable closest;
            if ((closest = GetClosestPickableItem()) != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    closest.Pickup();
                }
            }

            TalkerBase controller;
            if (controller = GetClosestDialogueController())
            {
                controller.Activate(true);
            }
        }

#if UNITY_EDITOR || UNITY_EDITOR_64
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position, fDamageDistance);
            Gizmos.color = triggerGizmoColor;
            Gizmos.DrawWireSphere(transform.position, fTriggerDistance);
        }
#endif
    }
}
