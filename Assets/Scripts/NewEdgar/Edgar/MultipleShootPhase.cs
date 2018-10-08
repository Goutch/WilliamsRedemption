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

        public override bool CanTransit()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Finish()
        {
            base.Finish();
        }

        protected override void Idle()
        {

        }

        public override void Act()
        {
            base.Act();

            if (numberProjectileShooted >= numberOfShoots && CurrentState.IsFinish())
            {
                Finish();
            }
        }

        public override void Transite()
        {
            base.Transite();
            animator.SetTrigger(R.S.AnimatorParameter.PlasmaShoot);

            Init();
        }

        public override void OnLeftSubState()
        {
            base.OnLeftSubState();

            ++numberProjectileShooted;
        }

        private void Init()
        {
            numberProjectileShooted = 0;
            lastTimeUsed = Time.time;
        }
    }
}
