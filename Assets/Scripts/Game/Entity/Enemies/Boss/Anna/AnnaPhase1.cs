using Game.Entity.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    [RequireComponent(typeof(RootMover))]
    class AnnaPhase1 : NonSequentialPhase
    {
        private RootMover mover;
        private Animator animator;

        private void Awake()
        {
            mover = GetComponent<RootMover>();
            animator = GetComponent<Animator>();
        }

        protected override void EnterIdle()
        {
            base.EnterIdle();
            animator.SetTrigger(Values.AnimationParameters.Anna.Walk);
        }

        protected override void Idle()
        {
            base.Idle();
            mover.LookAtPlayer();

            if (Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) > 0.25f)
            {
                mover.MoveForward();

                animator.SetBool(Values.AnimationParameters.Anna.Walking, true);
            }
            else
            {
                animator.SetBool(Values.AnimationParameters.Anna.Walking, false);
            }

        }

        public override bool CanEnter()
        {
            return true;
        }
    }
}
