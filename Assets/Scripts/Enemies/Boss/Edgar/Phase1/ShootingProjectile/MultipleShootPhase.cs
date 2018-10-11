using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boss;
using Harmony;
using UnityEngine;

namespace Edgar
{
    class MultipleShootPhase : Phase
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.PlasmaShoot + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private int numberOfShoots;
        [SerializeField] private float cooldown;

        private float lastTimeUsed;
        private int numberProjectileShooted = 0;

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        protected override void CurrentState_OnStateFinish(Boss.State state, State nextState)
        {
            base.CurrentState_OnStateFinish(state, nextState);
            ++numberProjectileShooted;

            if (numberProjectileShooted >= numberOfShoots)
            {
                Finish();
            }
        }

        protected override void EnterIdle()
        {
            animator.SetTrigger(R.S.AnimatorParameter.PlasmaShoot);
        }

        protected override void Idle() { }

        protected override void Initialise()
        {
            numberProjectileShooted = 0;
            lastTimeUsed = Time.time;
        }
    }
}
