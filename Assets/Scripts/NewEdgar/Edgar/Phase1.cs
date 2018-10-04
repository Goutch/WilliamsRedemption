using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boss;
using UnityEngine;

namespace Edgar
{
    class Phase1 : Phase
    {
        private const string ANIMATOR_TRIGGER = "IdlePhase1";
        [SerializeField] private Animator animator;

        public override bool CanTransit()
        {
            return false;
        }

        protected override void Idle()
        {

        }

        public override void Transit()
        {
            base.Transit();
            animator.SetTrigger(ANIMATOR_TRIGGER);
        }
    }
}
