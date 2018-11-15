using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(SpawnedTilesManager), typeof(RootMover))]
    class Phase2 : NonSequentialPhase
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.IdlePhase1 + "' ")]
        [SerializeField] private Animator animator;

        private SpawnedTilesManager spawnedTilesManager;
        private RootMover mover;

        protected override void Init()
        {
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();
            mover = GetComponent<RootMover>();
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

        public override void Enter()
        {
            base.Enter();
            spawnedTilesManager.DestroyAllTiles();
        }
    }
}
