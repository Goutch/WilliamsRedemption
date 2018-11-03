﻿using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(SpawnedTilesManager))]
    class DestroyTilesPhase : NonSequentialPhase
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.IdlePhase2 + "' ")]
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

            animator.SetTrigger(Values.AnimationParameters.Edgar.IdlePhase2);
        }
    }
}