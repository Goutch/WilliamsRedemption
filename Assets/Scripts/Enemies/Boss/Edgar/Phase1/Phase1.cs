using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Edgar
{
    [RequireComponent(typeof(Health), typeof(RootMover))]
    class Phase1 : Phase
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.IdlePhase1 + "' ")]
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
            health.OnHealthChange -= Health_OnHealthChange;
            if (health.HealthPoints / (float)health.MaxHealth <= percentageHealthTransitionCondition)
                Finish();
        }

        public override bool CanEnter()
        {
            return true;
        }

        protected override void EnterIdle()
        {
            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase1);
        }
        protected override void Idle()
        {
            base.Idle();

            mover.LookAtPlayer();
        }
    }
}
