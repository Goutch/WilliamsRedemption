using Boss;
using Harmony;
using UnityEngine;

namespace Jacob
{
    public class JacobPhase : Phase
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.IdlePhase1 + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private Capacity passiveCapacity;
        private State saveState;

        public override bool CanEnter()
        {
            return true;
        }
        public override void Enter()
        {
            base.Enter();
            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase1);
        }

        protected override void EnterIdle()
        {
            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase1);
        }

        public override void Act()
        {
            if((passiveCapacity?.CanEnter() ?? false) && !(CurrentState is Vulnerable))
            {
                saveState = CurrentState;
                CurrentState = passiveCapacity;

                passiveCapacity.OnStateFinish += PassiveCapacity_OnStateFinish;
            }

            base.Act();
        }

        private void PassiveCapacity_OnStateFinish(State state, State nextState)
        {
            passiveCapacity.OnStateFinish -= PassiveCapacity_OnStateFinish;
            CurrentState = saveState;
        }
    }
}
