using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class RecuperationPhase : SequentialPhase
    {
        [SerializeField] private float inactiveTimeBeforeUseCapacity;

        private Animator animator;

        private float lastTimeUsed;
        protected override void Init()
        {
            animator = GetComponent<Animator>();
        }

        public override void Act()
        {
            if(Time.time - lastTimeUsed > inactiveTimeBeforeUseCapacity)
                base.Act();
        }

        protected override void EnterIdle()
        {
            base.EnterIdle();
            animator.SetTrigger(Values.AnimationParameters.Jean.Idle);
        }

        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(Values.AnimationParameters.Jean.Idle);
            lastTimeUsed = Time.time;
        }
    }
}
