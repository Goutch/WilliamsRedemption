using Game.Puzzle.Light;
using System.Collections;
using System.Collections.Generic;
using Game.Audio;
using Game.Controller;
using Game.Controller.Events;
using UnityEngine;
using Game.Entity.Enemies.Attack;

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

        private LightSensor lightSensor;
        public KinematicRigidbody2D kRigidBody { get; private set; }
        private LayerMask layerMask;
        private Vector2 horizontalDirection;
        private Vector2 verticalDirection;

        [SerializeField] private AudioClip woundedSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        [SerializeField] private GameObject dmgEffect;

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

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            playerHorizontalDirection = Vector2.right;
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

        public void DamagePlayer(GameObject attacker)
        {
            if (!IsInvincible)
            {
                SoundCaller.CallSound(woundedSound, soundToPlayPrefab, gameObject, true);
                health.Hit(attacker);
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

        private bool HandleCollision(HitStimulus other)
        {
            if (!IsInvincible&&other.Type == HitStimulus.DamageType.Enemy)
            {
                DamagePlayer(other.gameObject);
                if (CurrentController is WilliamController)
                    Bleed(other);
                return true;
            }

            return false;
        }

        protected void Bleed(HitStimulus hitStimulus)
        {
            if (dmgEffect != null)
                Destroy(Instantiate(dmgEffect, hitStimulus.transform.position, hitStimulus.transform.rotation), 5);
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