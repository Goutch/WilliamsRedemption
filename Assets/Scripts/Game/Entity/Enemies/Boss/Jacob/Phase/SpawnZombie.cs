﻿using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jacob
{
    class SpawnZombie : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Jacob.IdlePhase + "' ")] [SerializeField]
        private Animator animator;

        [Tooltip("Require health component")] [SerializeField]
        private GameObject zombiePrefab;

        [SerializeField] private float cooldown;


        private float lastTimeUsed;

        private SpawnedEnemyManager spawnedEnemyManager;

        protected override void Init()
        {
            spawnedEnemyManager = GetComponent<SpawnedEnemyManager>();
        }

        public override void Act()
        {
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown && !spawnedEnemyManager.IsAllEnemySpawned())
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();

            lastTimeUsed = Time.time;
            animator.SetTrigger(Values.AnimationParameters.Jacob.IdlePhase);

            SpawnAZombie();

            Finish();
        }

        private void SpawnAZombie()
        {
            spawnedEnemyManager.SpawnEnemy(zombiePrefab, transform.position, Quaternion.identity);
        }
    }
}