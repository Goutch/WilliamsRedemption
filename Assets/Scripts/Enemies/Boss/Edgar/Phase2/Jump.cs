using System.Linq;
using Harmony;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Playmode.EnnemyRework.Boss.Edgar
{
    [RequireComponent(typeof(RootMover), typeof(Rigidbody2D), typeof(SpawnedTilesManager))]
    class Jump : Capacity
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.Jump + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [Tooltip("The distance between the pivot point of the boss and the tiles spawned on the Y axis.")]
        [SerializeField] private int yOffSetTileToSpawn;
        [SerializeField] private Tile tileToSpawn;
        [SerializeField] private float jumpDuration;
        [SerializeField] private State stateAfterJump;

        private Vector3Int[] spawnedTileRelativePositions = new Vector3Int[] {
            new Vector3Int(2, 1 ,0),
            new Vector3Int(-2, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0) };

        private RootMover rootMover;
        private Rigidbody2D rb;
        private SpawnedTilesManager spawnedTilesManager;

        private float lastTimeCapacityUsed;
        private float speed;

        private void Awake()
        {
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();

            rootMover = GetComponent<RootMover>();
            rb = GetComponent<Rigidbody2D>();

            if (capacityUsableAtStart)
                lastTimeCapacityUsed = -cooldown;
        }

        public override void Act()
        {
            rootMover.MoveOnXAxis();

            if (rb.velocity.y == 0)
            {
                Finish(stateAfterJump);
                rb.velocity = new Vector2();
            }
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeCapacityUsed > cooldown)
                return true;
            else
                return false;
        }
        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(R.S.AnimatorParameter.Jump);
            lastTimeCapacityUsed = Time.time;

            SetNewSpeed(PlayerController.instance.transform.position, jumpDuration);

            rootMover.Jump();
        }
        private void SetNewSpeed(Vector2 targetPoint, float duration)
        {
            float distance = targetPoint.x - transform.position.x;
            float speed = distance / duration;
            rootMover.Speed = speed;
        }

        public override void Finish(State nextState)
        {
            SpawnTiles();

            base.Finish(nextState);
        }
        private void SpawnTiles()
        {
            Vector3Int cellPos = spawnedTilesManager.ConvertLocalToCell(transform.position);
            cellPos.y += yOffSetTileToSpawn;

            spawnedTilesManager.SpawnTiles(cellPos, spawnedTileRelativePositions.ToList<Vector3Int>(), tileToSpawn);
        }
    }
}
