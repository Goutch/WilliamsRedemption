using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class ShootingPhase : SequentialPhase
    {
        [SerializeField] private float cooldownBetweenAttacks;

        private ShieldManager shieldManager;
        private Animator animator;
        private RootMover mover;

        private float lastAttack;

        public override bool CanEnter()
        {
            return base.CanEnter() && shieldManager.ShieldPercent > 0 && Time.time - lastAttack > cooldownBetweenAttacks;
        }

        protected override void Init()
        {
            mover = GetComponent<RootMover>();
            shieldManager = GetComponent<ShieldManager>();
            animator = GetComponent<Animator>();
        }

        protected override bool CanSwitchState()
        {
            return Time.time - lastAttack > cooldownBetweenAttacks && base.CanSwitchState();
        }

        protected override void CurrentState_OnStateFinish(State state)
        {
            base.CurrentState_OnStateFinish(state);
            lastAttack = Time.time;
        }

        public override void Enter()
        {
            mover.LookAtPlayer();
            base.Enter();
        }

        public override void Act()
        {
            mover.LookAtPlayer();

            base.Act();

            if (shieldManager.ShieldPercent == 0)
                Finish();
        }

        protected override void EnterIdle()
        {
            base.EnterIdle();
            animator.SetTrigger(Values.AnimationParameters.Jean.Idle);
        }
    }
}