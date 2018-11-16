using Game.Puzzle.Light;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Player
{
    public delegate void PlayerDeathEventHandler();

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Tooltip("Layers William collides with.")]
        private LayerMask williamLayerMask;

        [SerializeField] [Tooltip("Layers Reaper collides with.")]
        private LayerMask reaperLayerMask;

        [SerializeField] private float invincibilitySeconds;
        private Health health;
        public static PlayerController instance;

        private WilliamController williamController;
        private ReaperController reaperController;
        public EntityController CurrentController { get; private set; }
        private EntityController currentController;

        private LightSensor lightSensor;
        public KinematicRigidbody2D kRigidBody { get; private set; }
        private LayerMask layerMask;
        private Vector2 horizontalDirection;
        private Vector2 verticalDirection;
        
        [SerializeField] private AudioClip woundedSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;


        private int currentLevel;
        private int numbOfLocks = 0;

        public LayerMask WilliamLayerMask
        {
            get { return williamLayerMask; }
            set { williamLayerMask = value; }
        }

        public LayerMask ReaperLayerMask
        {
            get { return reaperLayerMask; }
            set { reaperLayerMask = value; }
        }

        public Vector2 playerHorizontalDirection
        {
            get { return horizontalDirection; }
            set { horizontalDirection = value; }
        }

        public Vector2 playerVerticalDirection
        {
            get { return verticalDirection; }
            set { verticalDirection = value; }
        }


        public bool IsOnGround => kRigidBody.IsGrounded;
        public bool IsDashing { get; set; }
        public bool IsMoving { get; set; }
        private bool isInvincible = false;

        public bool IsInvincible
        {
            get { return isInvincible; }
            set
            {
                isInvincible = value;
                williamController.animator.SetBool("Invincible", value);
                reaperController.animator.SetBool("Invincible", value);
            }
        }

        public FacingSideUpDown DirectionFacingUpDown { get; set; }
        public FacingSideLeftRight DirectionFacingLeftRight { get; set; }

        private void Awake()
        {
            currentLevel = 1;
            if (instance == null)
                instance = this;

            kRigidBody = GetComponent<KinematicRigidbody2D>();
            layerMask = kRigidBody.LayerMask;


            health = GetComponent<Health>();
            williamController = GetComponentInChildren<WilliamController>();
            reaperController = GetComponentInChildren<ReaperController>();
            GetComponent<HitSensor>().OnHit += HandleCollision;

            lightSensor = GetComponent<LightSensor>();
            lightSensor.OnLightExpositionChange += OnLightExpositionChanged;
            IsMoving = false;
        }

        private void Start()
        {
            OnLightExpositionChanged(false);
        }

        private void Update()
        {
            SetSpriteOrientation();
        }

        public void DamagePlayer()
        {
            if (!IsInvincible)
            {
                UseSound();
                health.Hit();
                StartCoroutine(InvincibleRoutine());
            }
        }

        private IEnumerator InvincibleRoutine()
        {
            IsInvincible = true;
            yield return new WaitForSeconds(invincibilitySeconds);
            IsInvincible = false;
        }

        private void HandleCollision(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Enemy ||
                other.DamageSource == HitStimulus.DamageSourceType.Obstacle)
            {
                DamagePlayer();
            }
        }


        private void OnLightExpositionChanged(bool exposed)
        {
            if (exposed)
                OnLightEnter();
            else
            {
                OnLightExit();
            }
        }

        public void OnLightEnter()
        {
            if (numbOfLocks == 0)
            {
                williamController.sprite.flipX = reaperController.sprite.flipX;
                williamController.gameObject.SetActive(true);
                reaperController.OnAttackFinish();
                CurrentController = williamController;
                reaperController.gameObject.SetActive(false);
                kRigidBody.LayerMask = williamLayerMask;
            }
        }

        public void OnLightExit()
        {
            if (numbOfLocks == 0)
            {
                reaperController.sprite.flipX = williamController.sprite.flipX;
                reaperController.gameObject.SetActive(true);
                williamController.OnAttackFinish();
                CurrentController = reaperController;
                williamController.gameObject.SetActive(false);
                kRigidBody.LayerMask = reaperLayerMask;
            }
        }

        public void LockTransformation()
        {
            numbOfLocks += 1;
        }

        public void UnlockTransformation()
        {
            if (numbOfLocks > 0)
                numbOfLocks -= 1;

            if (numbOfLocks == 0)
                OnLightExpositionChanged(lightSensor.InLight);
        }

        private void SetSpriteOrientation()
        {
            if (horizontalDirection == Vector2.left)
            {
                CurrentController.sprite.flipX = true;
            }
            else
            {
                CurrentController.sprite.flipX = false;
            }
        }
        private void UseSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,this.transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(woundedSound, true, this.gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}