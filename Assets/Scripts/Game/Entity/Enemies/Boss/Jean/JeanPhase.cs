using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class JeanPhase : NonSequentialPhase
    {
        private RootMover mover;
        private Animator animator;

        protected override void Init()
        {
            mover = GetComponent<RootMover>();
            animator = GetComponent<Animator>();
        }

        public override void Act()
        {
            mover.LookAtPlayer();
            base.Act();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override bool CanEnter()
        {
            return true;
        }

        protected override void EnterIdle()
        {
            base.EnterIdle();
            animator.SetTrigger(Values.AnimationParameters.Jean.Idle);
        }
    }
}