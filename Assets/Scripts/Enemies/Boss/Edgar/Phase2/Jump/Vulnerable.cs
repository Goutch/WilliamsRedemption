using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

namespace Edgar
{
    class Vulnerable : Boss.State
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.Vulnerable + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float duration;

        private float timeEntered;

        public override void Act()
        {
            if (Time.time - timeEntered > duration)
                Finish();
        }

        public override bool CanEnter()
        {
            return true;
        }

        protected override void Initialise()
        {
            timeEntered = Time.time;
            animator.SetTrigger(R.S.AnimatorParameter.Vulnerable);
        }
    }
}
