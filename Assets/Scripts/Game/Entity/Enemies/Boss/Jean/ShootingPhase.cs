using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class ShootingPhase : SequentialPhase
    {
        [SerializeField] private float cooldownBetweenAttacks;
        private ShieldManager shieldManager;
        private RootMover mover;

        private float lastAttack;

        public override bool CanEnter()
        {
            return base.CanEnter() && shieldManager.ShieldPercent > 0;
        }

        protected override void Init()
        {
            mover = GetComponent<RootMover>();
            shieldManager = GetComponent<ShieldManager>();
        }

        public override void Enter()
        {
            mover.LookAtPlayer();
            base.Enter();

            lastAttack = Time.time;
        }

        public override void Act()
        {
            mover.LookAtPlayer();

            TransiteToNextStateIfReady();
            base.Act();

            if (shieldManager.ShieldPercent == 0 && currentState == null)
                Finish();
        }

        protected override void CurrentState_OnStateFinish(State state)
        {
            state.OnStateFinish -= CurrentState_OnStateFinish;

            currentState = null;

            currentIndex++;

            if (currentIndex == subStates.Length)
            {
                Finish();
            }
        }


        protected override void TransiteToNextStateIfReady()
        {
            if (Time.time - lastAttack > cooldownBetweenAttacks && subStates[currentIndex].CanEnter() &&
                currentState == null)
            {
                if (IsIdling)
                    ExitIdle();

                lastAttack = Time.time;

                currentState = subStates[currentIndex];

                currentState.OnStateFinish += CurrentState_OnStateFinish;
                currentState.Enter();
            }
        }
    }
}