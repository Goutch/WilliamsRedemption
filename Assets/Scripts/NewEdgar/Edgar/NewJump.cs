using System;
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
        private Tilemap platforms;
        private LightController lightController;
        private Vector3Int[] spawnedTileRelativePositions = new Vector3Int[] {
            new Vector3Int(2, 1 ,0),
            new Vector3Int(-2, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0) };

        private new void Awake()
        {
            base.Awake();
            platforms = GameObject.FindGameObjectWithTag(R.S.Tag.Plateforme).GetComponent<Tilemap>();
            lightController = GameObject.FindGameObjectWithTag(R.S.Tag.LightManager).GetComponent<LightController>();

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

        public override bool CanTransit()
        {
            if (Time.time - lastTimeCapacityUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Transite()
        {
            base.Transite();
            animator.SetTrigger(R.S.AnimatorParameter.Jump);
            lastTimeCapacityUsed = Time.time;
            rootMover.Jump();
        }

        public override void Finish()
        {
            base.Finish();

            SpawnTiles();
        }

        private void SpawnTiles()
        {
            Vector3Int cellPos = platforms.LocalToCell(transform.position);
            cellPos.y += yOffSetTileToSpawn;

            List<Vector3Int> platformsPosition = new List<Vector3Int>();

            for (int i = 0; i < spawnedTileRelativePositions.Length; ++i)
            {
                Vector3Int tilePosition = new Vector3Int(cellPos.x + spawnedTileRelativePositions[i].x, cellPos.y + spawnedTileRelativePositions[i].y, 0);
                if (platforms.GetTile(tilePosition) == null)
                {
                    platforms.SetTile(tilePosition, tileToSpawn);
                    platformsPosition.Add(tilePosition);
                }
            }

            lightController.UpdateLightAtEndOfFrame();
        }
    }
}
