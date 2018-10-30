using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(RootMover))]
    class MultipleShootPhase : NonSequentialPhase
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.PlasmaShoot + "' ")]
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

        }

        protected override void CurrentState_OnStateFinish(Boss.State state)
        {
            base.CurrentState_OnStateFinish(state);
            ++numberProjectileShooted;

            if (numberProjectileShooted >= numberOfShoots)
            {
                Finish();
            }
        }

    }
}
