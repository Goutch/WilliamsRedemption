﻿using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(RootMover))]
    class MultipleShootPhase : NonSequentialPhase
    {
        [SerializeField] private int numberOfShoots;
        [SerializeField] private float cooldown;

        private RootMover rootMover;

        private float lastTimeUsed;
        private int numberProjectileShooted = 0;

        protected override void Init()
        {
            rootMover = GetComponent<RootMover>();
        }

        public override void Act()
        {
            rootMover.LookAtPlayer();

            base.Act();
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();
            numberProjectileShooted = 0;
            lastTimeUsed = Time.time;
        }

        protected override void EnterIdle()
        {
            base.EnterIdle();
        }

        protected override void CurrentState_OnStateFinish(State state)
        {
            base.CurrentState_OnStateFinish(state);
            ++numberProjectileShooted;

            if (numberProjectileShooted >= numberOfShoots)
            {
                Finish();
            }
        }
    }
}