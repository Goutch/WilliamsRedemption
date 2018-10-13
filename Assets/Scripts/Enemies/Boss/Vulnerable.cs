using Harmony;
using UnityEngine;

namespace Boss
{
    class Vulnerable : State
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
        public override void Enter()
        {
            base.Enter();

            timeEntered = Time.time;
            animator.SetTrigger(R.S.AnimatorParameter.Vulnerable);
        }
    }
}
