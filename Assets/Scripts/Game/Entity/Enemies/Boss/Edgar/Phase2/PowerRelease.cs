using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Entity.Enemies.Boss.Edgar
{
    class PowerRelease : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.IdlePhase1 + "' ")] [SerializeField]
        private Animator animator;

        [SerializeField] private Collider2D laserSpawnPointsZone;
        [SerializeField] private GameObject lasePrefab;
        [SerializeField] private float delayBetweenEachLaser;
        [SerializeField] private int numberOfLasersToSpawn;

        private int numberOfLaserFinish;

        private float leftBorderSpawnLaser;
        private float rightBorderSpawnLaser;
        private float positionYLaser;

        private SpawnedTilesManager spawnedTilesManager;

        protected override void Init()
        {
            spawnedTilesManager = GetComponent<SpawnedTilesManager>();

            Vector2 size = laserSpawnPointsZone.bounds.size;
            Vector2 center = laserSpawnPointsZone.bounds.center;

            leftBorderSpawnLaser = -size.x / 2 + center.x;
            rightBorderSpawnLaser = size.x / 2 + center.x;

            positionYLaser = center.y;
        }

        public override void Act()
        {
            Finish();
        }

        public override bool CanEnter()
        {
            return true;
        }

        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(Values.AnimationParameters.Edgar.IdlePhase1);
            numberOfLaserFinish = 0;
            StartCoroutine(SpawnLaser());
        }

        private IEnumerator SpawnLaser()
        {
            for (int numberSpawnerLaser = 0; numberSpawnerLaser < numberOfLasersToSpawn; ++numberSpawnerLaser)
            {
                yield return new WaitForSeconds(delayBetweenEachLaser);

                float positionX;
                do
                {
                    positionX = Random.Range(leftBorderSpawnLaser, rightBorderSpawnLaser);
                } while (spawnedTilesManager.IsAnySpawnedTiles(position =>
                    spawnedTilesManager.ConvertLocalToCell(new Vector2(positionX, 0)).x == position.Key.x));


                Instantiate(lasePrefab, new Vector2(positionX, positionYLaser), Quaternion.identity);
            }
        }
    }
}