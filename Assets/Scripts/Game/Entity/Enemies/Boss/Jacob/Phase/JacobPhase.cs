using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jacob
{
    public class JacobPhase : NonSequentialPhase
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Jacob.IdlePhase + "' ")]
        [SerializeField] private Animator animator;

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

        protected override void Init()
        {

        }
    }
}
