using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar
{
    public class EdgarController : MonoBehaviour
    {
        [SerializeField] private float cdHorizontaleSwing;
        [SerializeField] private float cdVerticalSwing;
        private float lastVerticalSwing = 0;
        private float lastHorizontaleSwing = 0;

        private Animator animator;
        private State currentState;
        public Collider2D Range { get; private set; }

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Range = GetComponent<Collider2D>();

            currentState = new Idle();
            currentState.Init(this);
        }

        private void Update()
        {
            currentState.Act();
        }

        public void SwingHorizontal()
        {
            animator.SetTrigger("HorizontalSwing");
            currentState = new HorizontalSwing();
            currentState.Init(this);

            lastHorizontaleSwing = Time.time;
        }

        public void SwingVertical()
        {
            animator.SetTrigger("VerticalSwing");
            currentState = new VerticalSwing();
            currentState.Init(this);

            lastVerticalSwing = Time.time;
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

        public void OnHorizontaleSwingFinish()
        {
            animator.SetTrigger("Idle");
            currentState = new Idle();
            currentState.Init(this);
        }

        public void OnVerticalSwingFinish()
        {
            animator.SetTrigger("Idle");
            currentState = new Idle();
            currentState.Init(this);
        }

        public void OnPlayerInRange()
        {
            if (CanHorizontaleSwing())
                SwingHorizontal();
        }
    }
}


