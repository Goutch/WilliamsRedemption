using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(Health), typeof(RootMover))]
    class Phase1 : NonSequentialPhase
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.IdlePhase1 + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float percentageHealthTransitionCondition;

        private Health health;
        private RootMover mover;

        private void Awake()
        {
            health = GetComponent<Health>();
            health.OnHealthChange += Health_OnHealthChange;

            mover = GetComponent<RootMover>();
        }

        private void Health_OnHealthChange(GameObject gameObject)
        {
            if (health.HealthPoints / (float)health.MaxHealth <= percentageHealthTransitionCondition)
            {
                health.OnHealthChange -= Health_OnHealthChange;
                Finish();
            }
        }

        public override bool CanEnter()
        {
            return true;
        }

        protected override void EnterIdle()
        {
            animator.SetTrigger(Values.AnimationParameters.Edgar.IdlePhase1);
        }
        protected override void Idle()
        {
            base.Idle();

            mover.LookAtPlayer();
        }
    }
}
