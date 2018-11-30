using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(SpawnedTilesManager), typeof(RootMover))]
    class HeavySwing : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.HeavySwing + "' ")] [SerializeField]
        private Animator animator;

        [SerializeField] private bool[] test;

        [SerializeField] private float cooldown;

        [Header("Sound")] [SerializeField] private AudioClip heavySwingSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private SpawnedTilesManager spawnedTilesManager;
        private RootMover mover;

        private float lastTimeCapacityUsed;

        protected override void Init()
        {
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();
            mover = GetComponent<RootMover>();
        }

        public override void Act()
        {
        }

        public void HeavySwingFinish()
        {
            spawnedTilesManager.DestroyAllTilesInFront();
            Finish();
        }

        public override void Finish()
        {
            base.Finish();
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeCapacityUsed > cooldown && spawnedTilesManager.IsAnySpawnedTiles())
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(Values.AnimationParameters.Edgar.HeavySwing);
            lastTimeCapacityUsed = Time.time;
            SoundCaller.CallSound(heavySwingSound, soundToPlayPrefab, gameObject, true);

            ChangeDirection();
        }

        private void ChangeDirection()
        {
            Vector3Int cellBossPosition = spawnedTilesManager.ConvertLocalToCell(transform.position);
            Func<KeyValuePair<Vector3Int, BirthLifeTime>, bool> positionsToTheLeftOfBoss = position => position.Key.x < cellBossPosition.x;
            Func< KeyValuePair<Vector3Int, BirthLifeTime>, bool> positionsToTheRightOfBoss = position => position.Key.x > cellBossPosition.x;

            mover.LookAtPlayer();

            float directionX = transform.rotation.y == -1 || transform.rotation.y == 1 ? -1 : 1;

            if (directionX > 0 && spawnedTilesManager.IsAnySpawnedTiles(positionsToTheRightOfBoss))
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else if (directionX > 0 && !spawnedTilesManager.IsAnySpawnedTiles(positionsToTheRightOfBoss))
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            else if (directionX < 0 && spawnedTilesManager.IsAnySpawnedTiles(positionsToTheLeftOfBoss))
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            else if (directionX < 0 && !spawnedTilesManager.IsAnySpawnedTiles(positionsToTheLeftOfBoss))
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
        }
    }
}