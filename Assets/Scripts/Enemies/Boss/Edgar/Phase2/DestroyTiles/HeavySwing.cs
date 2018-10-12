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

        private new void Awake()
        {
            base.Awake();
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();
        }

        public override void Act()
        {

        }

        public void HeavySwingFinish()
        {
            Vector3Int cellPlayerPosition = spawnedTilesManager.ConvertLocalToCell(PlayerController.instance.transform.position);
            Vector3Int cellBossPosition = spawnedTilesManager.ConvertLocalToCell(transform.position);

            if (directionX < 0)
                spawnedTilesManager.DestroyAllTilesToTheLeft(cellBossPosition);
            else if (directionX > 0)
                spawnedTilesManager.DestroyAllTilesToTheRight(cellBossPosition);

            Finish();
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeCapacityUsed > cooldown && spawnedTilesManager.IsAnyTilesSpawned())
                return true;
            else
                return false;
        }

        protected override void Initialise()
        {
            animator.SetTrigger(R.S.AnimatorParameter.HeavySwing);
            lastTimeCapacityUsed = Time.time;

            directionX = transform.rotation == Quaternion.AngleAxis(0, Vector3.up) ? -1 : 1;
            Vector3Int cellBossPosition = spawnedTilesManager.ConvertLocalToCell(transform.position);

            if (directionX < 0 && spawnedTilesManager.IsAnySpawnedTilesToLeftOfPosition(cellBossPosition))
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
            }
            else if (directionX < 0 && !spawnedTilesManager.IsAnySpawnedTilesToLeftOfPosition(cellBossPosition))
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
                directionX = 1;
            }
            else if (directionX > 0 && spawnedTilesManager.IsAnySpawnedTilesToRightOfPosition(cellBossPosition))
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            }
            else if (directionX > 0 && !spawnedTilesManager.IsAnySpawnedTilesToRightOfPosition(cellBossPosition))
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
                directionX = -1;
            }
        }
    }
}
