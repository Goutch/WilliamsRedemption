﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Edgar
{
    public class EdgarController : MonoBehaviour
    {
        [SerializeField] private float cdHorizontaleSwing;
        [SerializeField] public float speed;
        [SerializeField] private float delayDestructionPlatforms;
        [SerializeField] private float cdVerticalSwing;
        [SerializeField] private float cdJump;
        [SerializeField] private float cdPlasmaShoot;
        [SerializeField] private int numbOfPlasmaShoot;
        [SerializeField] private float delayBetweenEachShoot;
        [SerializeField] private GameObject bullet;
        [SerializeField] private float jumpForce;
        [SerializeField] private GameObject groundPlasma;
        [SerializeField] private Tile spawnTile;
        [SerializeField] private float upwardForceOnLandingWhenPlayerIsInAir;
        [SerializeField] private float upwardForceOnLandingWhenPlayerIsOnGround;
        [SerializeField] private int hp;
        [SerializeField] private GameObject leftFoot;
        [SerializeField] private GameObject rightFoot;
        [SerializeField] private Collider2D range;
        [SerializeField] float percentageForTransition;
        [SerializeField] private DoorScript door;
        private int currentPhase = 1;
        private Tilemap plateforms;

        private float lastVerticalSwing = 0;
        private float lastHorizontaleSwing = 0;
        private float lastJump = 0;
        private float lastPlasmaShoot = 0;

        private Animator animator;
        private State currentState;
        private MaceController maceController;
        public Collider2D Range { get; set; }
        public Rigidbody2D rb { get; private set; }

        public int Hp
        {
            get
            {
                return hp;
            }

            set
            {
                hp = value;
                if (hp / 100.0f <= percentageForTransition && currentPhase == 1)
                    TransitionToIdlePhase2State();
                if (hp <= 0)
                {
                    door.Open();
                    Debug.Log(door);
                }
            }
        }
        private LightController lightController;
        public HitSensor hitSensor;

        private void Awake()
        {
            plateforms = GameObject.FindGameObjectWithTag("Plateforme").GetComponent<Tilemap>();
            animator = GetComponent<Animator>();
            Range = range;
            rb = GetComponent<Rigidbody2D>();
            maceController = GetComponentInChildren<MaceController>();
            lightController = GameObject.FindGameObjectWithTag("LightManager").GetComponent<LightController>();
            hitSensor = GetComponent<HitSensor>();

            lastVerticalSwing = Time.time - cdVerticalSwing;
            lastHorizontaleSwing = Time.time - cdHorizontaleSwing;
            lastJump = Time.time - cdJump;
            lastPlasmaShoot = Time.time - cdPlasmaShoot;

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
            Instantiate(bullet, transform.position, direction);
        }
    }
}

