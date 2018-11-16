using Game.Puzzle.Light;
using System.Collections;
using System.Collections.Generic;
using Game.Controller;
using Game.Controller.Events;
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
        private PlayerHealthEventChannel playerHealthEventChannel;
        private GameController gameController;
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
        private bool isInvincible;

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
            {
                instance = this;
            }

            gameController = GameObject.FindGameObjectWithTag(Values.GameObject.GameController)
                .GetComponent<GameController>();
            playerHorizontalDirection = Vector2.right;
            kRigidBody = GetComponent<KinematicRigidbody2D>();
            layerMask = kRigidBody.LayerMask;

            isInvincible = false;
            numbOfLocks = 0;
        
            health = GetComponent<Health>();
            williamController = GetComponentInChildren<WilliamController>();
            reaperController = GetComponentInChildren<ReaperController>();
            CurrentController = williamController;
            lightSensor = GetComponent<LightSensor>();
            lightSensor.OnLightExpositionChange += OnLightExpositionChanged;
            GetComponent<HitSensor>().OnHit += HandleCollision;
            playerHealthEventChannel = gameController.GetComponent<PlayerHealthEventChannel>();
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
                health.Hit();
                StartCoroutine(InvincibleRoutine());
                playerHealthEventChannel.Publish(new OnPlayerTakeDamage());
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
                //williamController.sprite.flipX = reaperController.sprite.flipX;
                reaperController.OnAttackFinish();
                reaperController.gameObject.SetActive(false);
                williamController.gameObject.SetActive(true);

                CurrentController = williamController;

                kRigidBody.LayerMask = williamLayerMask;
            }
        }

        public void OnLightExit()
        {
            if (numbOfLocks == 0)
            {
                // reaperController.sprite.flipX = williamController.sprite.flipX;
                williamController.OnAttackFinish();
                williamController.gameObject.SetActive(false);
                reaperController.gameObject.SetActive(true);
                

                CurrentController = reaperController;

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
    }
}