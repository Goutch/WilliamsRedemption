using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Edgar
{
    [RequireComponent(typeof(RootMover))]
    class MultipleShootPhase : Phase
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.PlasmaShoot + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private int numberOfShoots;
        [SerializeField] private float cooldown;

        private RootMover rootMover;

        private float lastTimeUsed;
        private int numberProjectileShooted = 0;

        private void Awake()
        {
            rootMover = GetComponent<RootMover>();
        }

        public override void Act()
        {
            rootMover.LookAtPlayer();

            base.Act();
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }
        public override void Enter()
        {
            base.Enter();
            numberProjectileShooted = 0;
            lastTimeUsed = Time.time;
        }
        protected override void EnterIdle()
        {
            animator.SetTrigger(R.S.AnimatorParameter.PlasmaShoot);
        }

        protected override void CurrentState_OnStateFinish(Boss.State state, State nextState)
        {
            base.CurrentState_OnStateFinish(state, nextState);
            ++numberProjectileShooted;

            if (numberProjectileShooted >= numberOfShoots)
            {
                Finish();
            }
        }

    }
}
