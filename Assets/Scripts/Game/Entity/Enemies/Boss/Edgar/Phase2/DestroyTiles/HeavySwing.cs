using System;
using UnityEngine;


namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(SpawnedTilesManager), typeof(RootMover))]
    class HeavySwing : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.HeavySwing + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private bool[] test;

        [SerializeField] private float cooldown;
        
        [Header("Sound")] [SerializeField] private AudioClip heavySwingSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;

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
            CallHeavySwingSound();

            ChangeDirection();
        }
        private void ChangeDirection()
        {
            Vector3Int cellBossPosition = spawnedTilesManager.ConvertLocalToCell(transform.position);
            Func<Vector3Int, bool> positionsToTheLeftOfBoss = position => position.x < cellBossPosition.x;
            Func<Vector3Int, bool> positionsToTheRightOfBoss = position => position.x > cellBossPosition.x;

            mover.LookAtPlayer();

            float directionX = transform.rotation.y == -1 ? -1 : 1;

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
        
        private void CallHeavySwingSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(heavySwingSound, true, gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}
