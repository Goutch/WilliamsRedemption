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
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.Jump + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [Tooltip("The distance between the pivot point of the boss and the tiles spawned on the Y axis.")]
        [SerializeField] private int yOffSetTileToSpawn;
        [SerializeField] private Tile tileToSpawn;
        [SerializeField] private float jumpDuration;
        [SerializeField] private float landingDelay;
        [SerializeField] private GameObject landingEffect;
        [SerializeField] private Transform landingEffectSpawnPoint;

        [Header("Sound")] [SerializeField] private AudioClip landingSound;
        [SerializeField] private GameObject soundToPlayPrefab;
        private GameObject soundToPlay;

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
                StartCoroutine(landing());
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

            SetNewSpeed(PlayerController.instance.transform.position, jumpDuration);

            rootMover.LookAtPlayer();

            rootMover.Jump();
        }
        private void SetNewSpeed(Vector2 targetPoint, float duration)
        {
            float distance = targetPoint.x - transform.position.x;
            float speed = distance / duration;
            rootMover.Speed = speed;
        }

        private IEnumerator landing()
        {            
            CallLandingSound();
            yield return new WaitForSeconds(landingDelay);
            Finish();
            Instantiate(landingEffect, landingEffectSpawnPoint.position, Quaternion.identity);
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

        private void CallLandingSound()
        {
            soundToPlay=Instantiate(soundToPlayPrefab,transform.position,Quaternion.identity);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().Init(landingSound, false, gameObject);
            soundToPlay.GetComponent<AudioManagerSpecificSounds>().PlaySound();
        }
    }
}
