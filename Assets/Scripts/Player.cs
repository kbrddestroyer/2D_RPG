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
        private static Player instance;
        public static Player Instance { get => instance; }

        [Header("Base settings")]
        [SerializeField, Range(0f, 25f)] private float fSpeed;
        [SerializeField] protected Attack[] attacks;
        [SerializeField, Range(0f, 1f)]  protected float fDamageDelay;
        [SerializeField, Range(0f, 10f)] private float fTriggerDistance;
        [SerializeField, Range(0f, 10f)] private float fMaxHP;
        [SerializeField] private LayerMask enemy;
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
        private List<TalkerBase> lDialogueController;
        protected MasterDialogueController masterController;
        private bool summoned = false;

        private float comboPassTime;
        private uint attackCount;

        public List<Item> itemsPickedUpInLevel = new List<Item>();

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
            instance = this;

            StartCoroutine(Blink());
            lDialogueController = FindObjectsOfType<TalkerBase>().ToList<TalkerBase>();

            if (!masterController)
                masterController = GameObject.FindObjectOfType<MasterDialogueController>();
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

        public void DealDamage(int attackID)
        {
            if (attackID >= attacks.Length)
                return;

            animator.ResetTrigger("attack");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attacks[attackID].Damage, enemy);

            foreach (Collider2D collider in colliders)
            {
                IDamagable damagable;
                if ((damagable = collider.GetComponent<IDamagable>()) != null)
                {
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
            if (!summoned)
                return;

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
                attack.DrawGizmo(transform.position);
            }

            Gizmos.color = triggerGizmoColor;
            Gizmos.DrawWireSphere(transform.position, fTriggerDistance);
        }
#endif
    }
}
