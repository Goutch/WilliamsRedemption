using Game.Entity.Player;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(RootMover), typeof(Rigidbody2D), typeof(SpawnedTilesManager))]
    class Jump : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.Jump + "' ")] [SerializeField]
        private Animator animator;

        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;

        [Tooltip("The distance between the pivot point of the boss and the tiles spawned on the Y axis.")]
        [SerializeField]
        private int yOffSetTileToSpawn;

        [SerializeField] private Tile tileToSpawn;
        [SerializeField] private float jumpDuration;
        [SerializeField] private GameObject landingEffect;
        [SerializeField] private Transform landingEffectSpawnPoint;
        [SerializeField] private GameObject landingAreaOfEffectDamage;
        [SerializeField] private float areaEffectDuration;


        [Header("Sound")] [SerializeField] private AudioClip landingSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private Vector3Int[] spawnedTileRelativePositions = new Vector3Int[]
        {
            new Vector3Int(2, 1, 0),
            new Vector3Int(-2, 1, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 0, 0)
        };

        private RootMover rootMover;
        private Rigidbody2D rb;
        private SpawnedTilesManager spawnedTilesManager;

        private float lastTimeCapacityUsed;
        private float speed;
        private bool collideWithWall;

        protected override void Init()
        {
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();

            rootMover = GetComponent<RootMover>();
            rb = GetComponent<Rigidbody2D>();

            if (capacityUsableAtStart)
                lastTimeCapacityUsed = -cooldown;
        }

        public override void Act()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1, 1 << LayerMask.NameToLayer(Values.Layers.Platform));
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.down* 1, Color.blue);

            if (rb.velocity.y == 0)
                collideWithWall = true;

            if (rb.velocity.y == 0 && hit.collider != null)
            {
                Land();
            }
            else if(rb.velocity.y != 0 && !collideWithWall)
            {
                rootMover.MoveOnXAxis();
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

            animator.SetTrigger(Values.AnimationParameters.Edgar.Jump);
            lastTimeCapacityUsed = Time.time;

            SetNewSpeed(player.transform.position, jumpDuration);

            rootMover.LookAtPlayer();

            rootMover.Jump();

            collideWithWall = false;
        }

        private void SetNewSpeed(Vector2 targetPoint, float duration)
        {
            float distance = targetPoint.x - transform.position.x;
            float speed = distance / duration;
            rootMover.Speed = speed;
        }

        private void Land()
        {
            SoundCaller.CallSound(landingSound, soundToPlayPrefab, gameObject, false);
            Finish();
            Instantiate(landingEffect, landingEffectSpawnPoint.position, Quaternion.identity);
            GameObject areaOfEffect = Instantiate(landingAreaOfEffectDamage, landingEffectSpawnPoint);
            Destroy(areaOfEffect, areaEffectDuration);
        }

        public override void Finish()
        {
            SpawnTiles();

            rb.velocity = new Vector2();
            base.Finish();
        }

        private void SpawnTiles()
        {
            Vector3Int cellPos = spawnedTilesManager.ConvertLocalToCell(transform.position);
            cellPos.y += yOffSetTileToSpawn;

            spawnedTilesManager.SpawnTiles(cellPos, spawnedTileRelativePositions.ToList<Vector3Int>(), tileToSpawn);
        }
    }
}