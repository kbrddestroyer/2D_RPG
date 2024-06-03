using GUIControllers;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GameControllers 
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IDamagable
    {
        private static Player instance;
        public static Player Instance { get => instance; }

        [Header("Base settings")]
        [SerializeField, Range(0f, 25f)] private float fSpeed;
        [SerializeField] protected Attack[] attacks;
        [SerializeField] private bool autoAttack;
        [SerializeField, Range(0f, 5f)]  protected float fDamageDelay;
        [SerializeField, Range(0f, 10f)] private float fTriggerDistance;
        [SerializeField, Range(0f, 50f)] private float fMaxHP;
        [SerializeField, Range(0f, 50f)] private float fCorrection;
        [SerializeField] protected LayerMask enemy;
        [SerializeField] private LayerMask pickables;
        [Header("Combat logic")]
        [SerializeField, Range(0f, 5f)] private float comboFallback;
        [Header("Required Components")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected new AudioSource audio;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [Header("Audio")]
        [SerializeField] private AudioClip stepSFX;
        [SerializeField] private AudioClip damagedSFX;

        [Header("Gizmos")]
        [SerializeField] private Color gizmoColor = new Color(0f, 0f, 0f, 1f);
        [SerializeField] private Color triggerGizmoColor = new Color(0f, 0f, 0f, 1f);

        public Trigger trigger = null;

        private float fHP;
        protected float fPassedDelayTime = 0.0f;
        private bool summoned = false;

        private float comboPassTime;
        private uint attackCount;
        protected float speedMultiplier = 1;

        public virtual float HP
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
                fHP = Mathf.Clamp(value, 0, fMaxHP);
                HPGUIController.Instance.HP = value / fMaxHP;
                if (value <= 0)
                {
                    this.enabled = false;
                }
            }
        }

        public float Correction
        {
            get => fCorrection;
        }

        protected virtual IEnumerator Blink()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(5f);
                animator.SetTrigger("blink");
            }
        }

        public bool ValidateInteractDistance(Vector3 position)
        {
            return Vector3.Distance(transform.position, position) < fTriggerDistance;
        }

        protected virtual void Awake()
        {
            instance = this;
            InventoryManager.Instance.NotifyOnSpawn();
            StartCoroutine(Blink());
        }

        protected virtual void Start()
        {
            animator.SetTrigger("summon");
        }

        public virtual void OnSummoned()
        {
            summoned = true;
        }

        protected virtual void OnEnable()
        {
            fHP = fMaxHP;
        }

        protected virtual void OnDisable()
        {
            if (HP <= 0)
                OnDeath();
            Destroy(this.gameObject, 1f);
        }

        public virtual void OnDeath()
        {
            animator.SetTrigger("death");

            InventoryManager.Instance.NotifyOnPlayerDeath();
        }

        protected virtual void OnDestroy()
        {
            if (HP <= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        protected virtual void ApplyDamageToEnemy(IDamagable entity, int attackID)
        {
            entity.HP -= attacks[attackID].Damage;
        }

        public virtual void DealDamage(int attackID)
        {
            if (attackID >= attacks.Length)
                return;

            animator.ResetTrigger("attack");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attacks[attackID].Distance + attacks[attackID].Dispersion, enemy);

            foreach (Collider2D collider in colliders)
            {
                IDamagable damagable;
                if ((damagable = collider.GetComponent<IDamagable>()) != null)
                {
                    if (attacks[attackID].validatePredicate(transform.position, damagable.Correction, collider.transform.position, spriteRenderer.flipX))
                        ApplyDamageToEnemy(damagable, attackID);
                }
            }
        }

        protected virtual void Attack()
        {
            if (fPassedDelayTime < fDamageDelay)
                return;
            fPassedDelayTime = 0.0f;
            animator.SetInteger("attackType", (int) attackCount % attacks.Length);
            animator.SetTrigger("attack");
            audio.PlayOneShot(attacks[attackCount % attacks.Length].SFX);
            attackCount++;
            comboPassTime = 0;
        }

        protected IMasterDialogue GetClosestMasterDialogue() 
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fTriggerDistance, pickables);

            if (colliders.Length == 0) return null;

            IMasterDialogue closest = colliders[0].GetComponent<IMasterDialogue>();
            float distance = Vector3.Distance(colliders[0].transform.position, transform.position);
            foreach (Collider2D collider in colliders)
            {
                IMasterDialogue pickable;
                if ((pickable = collider.GetComponent<IMasterDialogue>()) != null)
                {
                    float curDistance = Vector2.Distance(transform.position, collider.transform.position);
                    if (curDistance < distance)
                    {
                        closest = pickable;
                        distance = curDistance;
                    }
                }
            }
            return closest;
        }

        public void Step()
        {
            audio.PlayOneShot(stepSFX);
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
            if (!summoned)
                return;

            Vector2 inputSpeed = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * fSpeed * speedMultiplier;  // Direction vector
            spriteRenderer.flipX = (inputSpeed.x == 0) ? spriteRenderer.flipX : inputSpeed.x < 0;

            UpdatePassedTimeGUI();

            animator.SetBool("walking", inputSpeed.magnitude > 0);
            transform.Translate(inputSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Mouse0) || (autoAttack && Input.GetKey(KeyCode.Mouse0)))
            {
                Attack();
            }

            IMasterDialogue closest;
            if ((closest = GetClosestMasterDialogue()) != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    closest.Interact();
                }
            }

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

#if UNITY_EDITOR || UNITY_EDITOR_64
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmoColor;

            foreach (Attack attack in attacks)
            {
                attack.DrawGizmo(transform.position, spriteRenderer.flipX);
            }

            Gizmos.color = triggerGizmoColor;
            Gizmos.DrawWireSphere(transform.position, fTriggerDistance);
        }
#endif
    }
}
