using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boss;
using Harmony;
using Playmode.EnnemyRework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Edgar
{
    [RequireComponent(typeof(RootMover), typeof(Rigidbody2D), typeof(SpawnedTilesManager))]
    class NewJump : Capacity
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.Jump + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [Tooltip("The distance between the pivot point of the boss and the tiles spawned on the Y axis.")]
        [SerializeField] private int yOffSetTileToSpawn;
        [SerializeField] private Tile tileToSpawn;

        private float lastTimeCapacityUsed;

        private RootMover rootMover;
        private Rigidbody2D rb;
        private SpawnedTilesManager spawnedTilesManager;
        private Vector3Int[] spawnedTileRelativePositions = new Vector3Int[] {
            new Vector3Int(2, 1 ,0),
            new Vector3Int(-2, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0) };

        private new void Awake()
        {
            base.Awake();
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();

            rootMover = GetComponent<RootMover>();
            rb = GetComponent<Rigidbody2D>();

            if (capacityUsableAtStart)
                lastTimeCapacityUsed = -cooldown;
        }

        public override void Act()
        {
            if (rb.velocity.y == 0)
                Finish();
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeCapacityUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Finish()
        {
            SpawnTiles();

            base.Finish();
        }

        private void SpawnTiles()
        {
            Vector3Int cellPos = spawnedTilesManager.ConvertLocalToCell(transform.position);
            cellPos.y += yOffSetTileToSpawn;

            spawnedTilesManager.SpawnTiles(cellPos, spawnedTileRelativePositions.ToList<Vector3Int>(), tileToSpawn);
        }

        protected override void Initialise()
        {
            animator.SetTrigger(R.S.AnimatorParameter.Jump);
            lastTimeCapacityUsed = Time.time;
            rootMover.Jump();
        }
    }
}
