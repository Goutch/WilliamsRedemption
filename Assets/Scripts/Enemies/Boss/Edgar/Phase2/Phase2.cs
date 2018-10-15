using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Edgar
{
    [RequireComponent(typeof(SpawnedTilesManager), typeof(RootMover))]
    class Phase2 : Phase
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.IdlePhase2 + "' ")]
        [SerializeField] private Animator animator;

        private SpawnedTilesManager spawnedTilesManager;
        private RootMover mover;

        private void Awake()
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
            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase2);
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
