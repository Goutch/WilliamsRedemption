using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boss;
using Harmony;
using UnityEngine;


namespace Edgar
{
    [RequireComponent(typeof(SpawnedTilesManager))]
    class HeavySwing : Capacity
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.HeavySwing + "' ")]
        [SerializeField] private Animator animator;

        [SerializeField] private float cooldown;

        private SpawnedTilesManager spawnedTilesManager;
        private float directionX;
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

            directionX = transform.rotation == Quaternion.AngleAxis(0, Vector3.up) ? -1 : 1;
            Vector3Int cellBossPosition = spawnedTilesManager.ConvertLocalToCell(transform.position);


            if (directionX < 0 && spawnedTilesManager.IsAnySpawnedTiles(position => position.x < cellBossPosition.x))
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else if (directionX < 0 && !spawnedTilesManager.IsAnySpawnedTiles(position => position.x < cellBossPosition.x))
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                directionX = 1;
            }
            else if (directionX > 0 && spawnedTilesManager.IsAnySpawnedTiles(position => position.x > cellBossPosition.x))
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            else if (directionX > 0 && !spawnedTilesManager.IsAnySpawnedTiles(position => position.x > cellBossPosition.x))
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                directionX = -1;
            }
        }
    }
}
