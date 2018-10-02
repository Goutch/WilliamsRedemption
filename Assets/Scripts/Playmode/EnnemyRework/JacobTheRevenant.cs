using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public class JacobTheRevenant : Enemy
    {
        [SerializeField] private Transform[] teleportPoints;

        [SerializeField] private GameObject zombiePrefab;

        [SerializeField] private float StunTimeInSecond;

        [SerializeField] private SpriteRenderer stunSprite;

        [SerializeField] private int numberEnemiesPerWave;

        [SerializeField] private float timeBetweenSpawns;

        [SerializeField] private float distanceFromPlayerTeleport;
        protected RootMover rootMover;
        protected SpriteRenderer spriteRenderer;
        protected int currenDirection = 1;
        private Rigidbody2D rigidbody2d;
        private bool stuned;

        private bool Stunned
        {
            get { return stuned; }
            set
            {
                stunSprite.gameObject.SetActive(value);
                stuned = value;
            }
        }

        private List<GameObject> spawnedZombies;

        protected override void Init()
        {
            spawnedZombies = new List<GameObject>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rootMover = GetComponent<RootMover>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidbody2d = GetComponent<Rigidbody2D>();
            StartCoroutine(SpawnWaveRoutine());
            Stunned = false;
        }

        public override void ReceiveDamage(Collider2D other)
        {
            if (stuned)
            {
                base.ReceiveDamage(other);
                Stunned = false;
            }
        }

        protected virtual void FixedUpdate()
        {
            

            if (Stunned)
                return;
            if (!IsSpawnedZombiesStillAlive())
            {
                StartCoroutine(StunnedRoutine());
            }

            Teleport(false);
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            if (PlayerController.instance.transform.position.x < this.transform.position.x)
            {
                spriteRenderer.flipX = true;
                currenDirection = -1;
            }
            else
            {
                spriteRenderer.flipX = false;
                currenDirection = 1;
            }
        }
        
        private void Teleport(bool forceTeleport)
        {
            for (int i = 0; i < teleportPoints.Length; i++)
            {
                if (forceTeleport)
                {
                    transform.position = teleportPoints[Random.Range(0, teleportPoints.Length)].position;
                }

                if (Vector2.Distance(transform.position, PlayerController.instance.transform.position) >
                    distanceFromPlayerTeleport)
                {
                    break;
                }

                if (!forceTeleport)
                    transform.position = teleportPoints[Random.Range(0, teleportPoints.Length)].position;
            }
        }

        private bool IsSpawnedZombiesStillAlive()
        {
            foreach (var zombie in spawnedZombies)
            {
                if (zombie != null)
                    return true;
            }

            return false;
        }

        IEnumerator StunnedRoutine()
        {
            Stunned = true;
            yield return new WaitForSeconds(StunTimeInSecond);
            Stunned = false;
            if (!IsSpawnedZombiesStillAlive())
            {
                StartCoroutine(SpawnWaveRoutine());
            }
        }

        IEnumerator SpawnWaveRoutine()
        {
            for (int i = 0; i < numberEnemiesPerWave; i++)
            {
                Teleport(true);
                UpdateDirection();
                GameObject zombie = Instantiate(zombiePrefab,
                    transform.position + Vector3.right * currenDirection * .16f, Quaternion.identity);
                spawnedZombies.Add(zombie);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }
}