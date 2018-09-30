using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Edgar
{
    public class EdgarController : MonoBehaviour
    {
        [SerializeField] private float cdHorizontaleSwing;
        [SerializeField] public float speed;
        [SerializeField] private float delayDestructionJumpPlatforms;
        [SerializeField] private float cdVerticalSwing;
        [SerializeField] private float cdJump;
        [SerializeField] private float jumpForce;
        [SerializeField] private GameObject groundPlasma;
        private Tilemap plateforms;
        [SerializeField] private Tile spawnTile;

        private float lastVerticalSwing = 0;
        private float lastHorizontaleSwing = 0;
        private float lastJump = 0;

        private Animator animator;
        private State currentState;
        private MaceController maceController;
        public Collider2D Range { get; private set; }
        public Rigidbody2D rb { get; private set; }
        private LightController lightController;

        private void Awake()
        {
            plateforms = GameObject.FindGameObjectWithTag("Plateforme").GetComponent<Tilemap>();
            animator = GetComponent<Animator>();
            Range = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            maceController = GetComponentInChildren<MaceController>();
            lightController = GameObject.FindGameObjectWithTag("LightManager").GetComponent<LightController>();

            lastVerticalSwing = Time.time - cdVerticalSwing;
            lastHorizontaleSwing = Time.time - cdHorizontaleSwing;
            lastJump = Time.time - cdJump;

            currentState = new IdlePhase2();
            currentState.Init(this);
        }

        private void Update()
        {
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

        public void OnHorizontaleSwingFinish()
        {
            TransitionToIdlePhase1State();
        }

        public void OnVerticalSwingFinish()
        {
            maceController.AttackWithPlasma();

            TransitionToIdlePhase1State();
        }

        public void OnPlayerInRange()
        {
            if (CanHorizontaleSwing())
                SwingHorizontal();
        }

        public void TransitionToIdlePhase1State()
        {
            animator.SetTrigger("IdlePhase1");
            currentState = new IdlePhase1();
            currentState.Init(this);
        }

        public void TransitionToIdlePhase2State()
        {
            animator.SetTrigger("IdlePhase2");
            Debug.Log("Transite to Phase 2");

            currentState = new IdlePhase2();
            currentState.Init(this);
        }


        public void TransitionToJumpState()
        {
            lastJump = Time.time;

            Debug.Log("Transite to Jump");
            animator.SetTrigger("Jump");
            currentState = new Jump(plateforms, spawnTile, lightController, delayDestructionJumpPlatforms);
            currentState.Init(this);
        }

        public void Jump()
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        public void OnJumpFinish()
        {
            TransitionToIdlePhase2State();

            PlayerController.instance.Rigidbody.AddForce(new Vector2(-1, 5), ForceMode2D.Impulse);
        }
    }
}


