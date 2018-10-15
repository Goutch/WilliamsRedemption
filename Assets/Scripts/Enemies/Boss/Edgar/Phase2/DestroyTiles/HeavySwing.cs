using Harmony;
using System;
using UnityEngine;


namespace Playmode.EnnemyRework.Boss.Edgar
{
    [RequireComponent(typeof(SpawnedTilesManager))]
    class HeavySwing : Capacity
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.HeavySwing + "' ")]
        [SerializeField] private Animator animator;

        [SerializeField] private float cooldown;

        private SpawnedTilesManager spawnedTilesManager;

        private float lastTimeCapacityUsed;

        private void Awake()
        {
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();
        }

        public override void Act()
        {

        }

        public void HeavySwingFinish()
        {
            spawnedTilesManager.DestroyAllTilesInFront();

            Finish();
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

            animator.SetTrigger(R.S.AnimatorParameter.HeavySwing);
            lastTimeCapacityUsed = Time.time;

            ChangeDirection();
        }
        private void ChangeDirection()
        {
            Vector3Int cellBossPosition = spawnedTilesManager.ConvertLocalToCell(transform.position);
            Func<Vector3Int, bool> positionsToTheLeftOfBoss = position => position.x < cellBossPosition.x;
            Func<Vector3Int, bool> positionsToTheRightOfBoss = position => position.x > cellBossPosition.x;

            float directionX = transform.rotation == Quaternion.AngleAxis(0, Vector3.up) ? -1 : 1;

            if (directionX < 0 && spawnedTilesManager.IsAnySpawnedTiles(positionsToTheLeftOfBoss))
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else if (directionX < 0 && !spawnedTilesManager.IsAnySpawnedTiles(positionsToTheLeftOfBoss))
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                directionX = 1;
            }
            else if (directionX > 0 && spawnedTilesManager.IsAnySpawnedTiles(positionsToTheRightOfBoss))
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            else if (directionX > 0 && !spawnedTilesManager.IsAnySpawnedTiles(positionsToTheRightOfBoss))
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                directionX = -1;
            }
        }
    }
}
