using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    class EdgarJump : Capacity
    {
        [SerializeField] private float cooldown;
        [SerializeField] private float duration;
        [SerializeField] private GameObject landingEffect;
        [SerializeField] private Transform landingEffectSpawnPoint;
        [SerializeField] private GameObject landingAreaOfEffectDamage;
        [SerializeField] private GameObject projectileSpawning;
        [SerializeField] private float areaEffectDuration;

        [Header("Sound")] [SerializeField] private AudioClip landingSound;
        [SerializeField] private GameObject soundToPlayPrefab;

        private RootMover mover;
        private Animator animator;
        private Rigidbody2D rb;
        private SpawnedTilesManager spawnedTilesManager;

        private float lastTimeCapacityUsed;
        private float speed;
        private bool collideWithWall;

        protected override void Init()
        {
            mover = GetComponent<RootMover>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();

            lastTimeCapacityUsed = -cooldown;
        }

        public override void Act()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1, 1 << LayerMask.NameToLayer(Values.Layers.Platform));
            Debug.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * 1, Color.blue);

            if (rb.velocity.y == 0)
                collideWithWall = true;

            if (rb.velocity.y == 0 && hit.collider != null)
            {
                Land();
            }
            else if (rb.velocity.y != 0 && !collideWithWall)
            {
                mover.MoveOnXAxis();
            }
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeCapacityUsed >= cooldown)
                return true;
            else
                return false;
        }

        public override void Finish()
        {
            base.Finish();
            lastTimeCapacityUsed = Time.time;
        }

        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(Values.AnimationParameters.Edgar.Jump);
            lastTimeCapacityUsed = Time.time;

            SetNewSpeed(player.transform.position, duration);

            mover.LookAtPlayer();

            mover.Jump();

            collideWithWall = false;
        }

        private void SetNewSpeed(Vector2 targetPoint, float duration)
        {
            float distance = targetPoint.x - transform.position.x;
            float speed = distance / duration;
            mover.Speed = speed;
        }

        private void Land()
        {
            SoundCaller.CallSound(landingSound, soundToPlayPrefab, gameObject, false);
            Finish();
            Instantiate(landingEffect, landingEffectSpawnPoint.position, Quaternion.identity);
            ShootProjectile(Quaternion.AngleAxis(0, Vector3.up));
            ShootProjectile(Quaternion.AngleAxis(180, Vector3.up));
            GameObject areaOfEffect = Instantiate(landingAreaOfEffectDamage, landingEffectSpawnPoint);
            Destroy(areaOfEffect, areaEffectDuration);

            spawnedTilesManager.DestroyAllTiles();
        }

        private void ShootProjectile(Quaternion rotation)
        {
            GameObject projectileObject =
                Instantiate(projectileSpawning, landingEffectSpawnPoint.transform.position, rotation);

            projectileObject.GetComponent<PlasmaGroundController>().Init(spawnedTilesManager);
        }
    }
}
