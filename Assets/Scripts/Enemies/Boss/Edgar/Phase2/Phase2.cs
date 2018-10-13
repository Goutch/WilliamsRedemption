using Boss;
using Harmony;
using UnityEngine;

namespace Edgar
{
    class Phase2 : Phase
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.IdlePhase2 + "' ")]
        [SerializeField] private Animator animator;
        private SpawnedTilesManager spawnedTilesManager;

        private void Awake()
        {
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();
        }

        public override bool CanEnter()
        {
            return false;
        }

        protected override void EnterIdle()
        {
            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase2);
        }

        protected override void Idle()
        {
            base.Idle();

            float directionX = Mathf.Sign(PlayerController.instance.transform.position.x - transform.position.x);
            if (directionX > 0)
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            else
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }

        public override void Enter()
        {
            base.Enter();
            spawnedTilesManager.DestroyAllTiles();
        }
    }
}
