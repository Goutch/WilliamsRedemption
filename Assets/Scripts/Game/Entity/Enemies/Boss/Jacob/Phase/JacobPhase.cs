using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jacob
{
    public class JacobPhase : NonSequentialPhase
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Jacob.IdlePhase + "' ")]
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
            animator.SetTrigger(Values.AnimationParameters.Jacob.IdlePhase);
        }

        protected override void EnterIdle()
        {
            animator.SetTrigger(Values.AnimationParameters.Jacob.IdlePhase);
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

        private void PassiveCapacity_OnStateFinish(State state)
        {
            passiveCapacity.OnStateFinish -= PassiveCapacity_OnStateFinish;
            CurrentState = saveState;
        }
    }
}
