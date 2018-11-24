using Game.Entity.Enemies.Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    class ShadowstepPhase : SequentialPhase
    {
        [SerializeField] private float cooldownBetweenCapacities;

        private float lastCapacityUsed;

        protected override void Init()
        {
        }

        public override void Enter()
        {
            base.Enter();

            lastCapacityUsed = int.MinValue;
        }

        protected override void TransiteToNextStateIfReady()
        {
            if (Time.time - lastCapacityUsed > cooldownBetweenCapacities)
                base.TransiteToNextStateIfReady();
        }

        protected override void CurrentState_OnStateFinish(State state)
        {
            base.CurrentState_OnStateFinish(state);
            lastCapacityUsed = Time.time;
        }
    }
}