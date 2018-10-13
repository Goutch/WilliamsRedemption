using Boss;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Edgar
{
    class DestroyTilesPhase : Phase
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
            if (spawnedTilesManager.IsAnySpawnedTiles())
                return true;
            else
                return false;
        }

        protected override void EnterIdle()
        {
            if (!spawnedTilesManager.IsAnySpawnedTiles())
                Finish();

            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase2);
        }
    }
}
