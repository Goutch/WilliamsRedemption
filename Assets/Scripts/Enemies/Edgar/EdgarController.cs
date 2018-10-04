using Playmode.EnnemyRework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Tilemaps;
namespace Edgar
{
    public class EdgarController : Enemy
    {
        [SerializeField] private float cdHorizontaleSwing;
        [SerializeField] private float delayDestructionPlatforms;
        [SerializeField] private float cdVerticalSwing;
        [SerializeField] private float cdJump;
        [SerializeField] private float cdPlasmaShoot;
        [SerializeField] private int numbOfPlasmaShoot;
        [SerializeField] private float delayBetweenEachShoot;
        [SerializeField] private GameObject bullet;
        [SerializeField] private float jumpForce;
        [SerializeField] private Tile spawnTile;
        [SerializeField] private float upwardForceOnLandingWhenPlayerIsInAir;
        [SerializeField] private float upwardForceOnLandingWhenPlayerIsOnGround;
        [SerializeField] private GameObject leftFoot;
        [SerializeField] private GameObject rightFoot;
        [SerializeField] float percentageForTransition;
        [SerializeField] private float meleeAttackRange;
        private int currentPhase = 1;
        private Tilemap plateforms;
    
        private float lastVerticalSwing = 0;
        private float lastHorizontaleSwing = 0;
        private float lastJump = 0;
        private float lastPlasmaShoot = 0;

        private Animator animator;
        private State currentState;
        private MaceController maceController;
        public Rigidbody2D rb { get; private set; }
        public bool PlayerIsInMeleeAttackRange=>Vector2.Distance(transform.position,PlayerController.instance.transform.position)<meleeAttackRange;

        public Tilemap Plateforms => plateforms;

        private int hp;
        public int Hp
        {
            get
            {
                return hp;
            }

            set
            {
                hp = value;
                if (Hp / (float) health.MaxHealth <= percentageForTransition && currentPhase == 1)
                    TransitionToIdlePhase2State();
                Debug.Log(hp);
            }
        }
        private LightController lightController;

        private new void Awake()
        {
            base.Awake();
            plateforms = GameObject.FindGameObjectWithTag("Plateforme").GetComponent<Tilemap>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            maceController = GetComponentInChildren<MaceController>();
            lightController = GameObject.FindGameObjectWithTag("LightManager").GetComponent<LightController>();

            lastVerticalSwing = Time.time - cdVerticalSwing;
            lastHorizontaleSwing = Time.time - cdHorizontaleSwing;
            lastJump = Time.time - cdJump;
            lastPlasmaShoot = Time.time - cdPlasmaShoot;

            hp = health.HealthPoints;

            currentState = new IdlePhase1();
            currentState.Init(this);
        }

        private void Update()
        {
            float directionX = Mathf.Sign(PlayerController.instance.transform.position.x - transform.position.x);
            if (directionX > 0)
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            else
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);

            currentState.Act();
        }

        public void SwingHorizontal()
        {
            lastHorizontaleSwing = Time.time;

            animator.SetTrigger("HorizontalSwing");
            currentState = new HorizontalSwing();
            currentState.Init(this);
        }

        public void SwingVertical()
        {
            lastVerticalSwing = Time.time;

            animator.SetTrigger("VerticalSwing");
            currentState = new VerticalSwing();
            currentState.Init(this);
        }

        public bool CanHorizontaleSwing()
        {
            if (Time.time - lastHorizontaleSwing > cdHorizontaleSwing)
                return true;
            else
                return false;
        }

        public bool CanVerticalSwing()
        {
            if (Time.time - lastVerticalSwing > cdVerticalSwing)
                return true;
            else
                return false;
        }

        public bool CanJump()
        {
            if (Time.time - lastJump > cdJump)
                return true;
            else
                return false;
        }

        public bool CanShootPlasma()
        {
            if (Time.time - lastPlasmaShoot > cdPlasmaShoot)
                return true;
            else
                return false;
        }

        public void OnHorizontaleSwingFinish()
        {
            TransitionToIdlePhase1State();
        }

        public void OnVerticalSwingFinish()
        {
            maceController.AttackWithPlasma(delayDestructionPlatforms);

            TransitionToIdlePhase1State();
        }

        public void OnPlayerInRange()
        {
            if (CanHorizontaleSwing())
                SwingHorizontal();
        }

        public void OnPlasmaShootFinish()
        {
            TransitionToIdlePhase1State();
        }

        public void TransitionToIdle()
        {
            if (currentPhase == 1)
                TransitionToIdlePhase1State();
            else
                TransitionToIdlePhase2State();
        }

        public void TransitionToIdlePhase1State()
        {
            currentPhase = 1;

            animator.SetTrigger("IdlePhase1");
            currentState = new IdlePhase1();
            currentState.Init(this);
        }

        public void TransitionToIdlePhase2State()
        {
            currentPhase = 2;

            animator.SetTrigger("IdlePhase2");

            currentState = new IdlePhase2();
            currentState.Init(this);
        }


        public void TransitionToJumpState()
        {
            lastJump = Time.time;

            animator.SetTrigger("Jump");
            currentState = new Jump(plateforms, 
                spawnTile, 
                lightController, 
                delayDestructionPlatforms, 
                leftFoot, 
                rightFoot, 
                upwardForceOnLandingWhenPlayerIsInAir, 
                upwardForceOnLandingWhenPlayerIsOnGround);

            currentState.Init(this);
        }

        public void TransitionToPlasmaShoot()
        {
            lastPlasmaShoot = Time.time;

            animator.SetTrigger("PlasmaShoot");
            currentState = new PlasmaShoot(numbOfPlasmaShoot, delayBetweenEachShoot);
            currentState.Init(this);
        }

        public void Jump()
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        public void OnJumpFinish()
        {
            TransitionToIdle();
        }

        public void ShootPlasma(Quaternion direction)
        {
            GameObject projectile = Instantiate(bullet, transform.position, direction);
            projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Ennemy);

        }

        protected override void Init()
        {
            health.OnHealthChange += Health_OnHealthChange;
        }

        private void Health_OnHealthChange()
        {
            Hp = health.HealthPoints;
        }

        protected override void HandleCollision(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper)
                health.Hit();
        }
    }
}


