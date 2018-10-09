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

        private new void Awake()
        {
            base.Awake();
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();
        }

        public override bool CanEnter()
        {
            if (spawnedTilesManager.IsAnyTilesSpawned())
                return true;
            else
                return false;
        }

        protected override void EnterIdle()
        {
            if (!spawnedTilesManager.IsAnyTilesSpawned())
                Finish();

            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase2);
        }

        protected override void Idle()
        {
            
        }

        protected override void Initialise()
        {
            
        }
    }
}
