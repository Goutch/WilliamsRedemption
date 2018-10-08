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
    class Phase2 : Phase
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.IdlePhase2 + "' ")]
        [SerializeField] private Animator animator;

        public override bool CanTransit()
        {
            return false;
        }

        protected override void Idle()
        {

        }

        public override void Transite()
        {
            base.Transite();
            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase2);
        }

        public override void OnLeftSubState()
        {
            base.OnLeftSubState();
            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase2);
        }
    }
}
