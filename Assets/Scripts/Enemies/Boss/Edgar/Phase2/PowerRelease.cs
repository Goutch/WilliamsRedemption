using UnityEngine;
using System.Collections;
using Harmony;

namespace Playmode.EnnemyRework.Boss.Edgar
{
    class PowerRelease : Capacity
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.PlasmaRelease + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D laserSpawnPointsZone;
        [SerializeField] private GameObject lasePrefab;
        [SerializeField] private float delayBetweenEachLaser;
        [SerializeField] private int numberOfLasersToSpawn;

        private int numberOfLaserFinish;

        private float leftBorderSpawnLaser;
        private float rightBorderSpawnLaser;
        private float positionYLaser;

        private void Awake()
        {
            Vector2 size = laserSpawnPointsZone.bounds.size;
            Vector2 center = laserSpawnPointsZone.bounds.center;

            leftBorderSpawnLaser = -size.x/2 + center.x;
            rightBorderSpawnLaser = size.x/2 + center.x;

            positionYLaser = center.y;
        }

        public override void Act()
        {

        }

        public override bool CanEnter()
        {
            return true;
        }
        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(R.S.AnimatorParameter.PlasmaRelease);
            numberOfLaserFinish = 0;
            StartCoroutine(SpawnLaser());
        }
        private IEnumerator SpawnLaser()
        {
            for(int numberSpawnerLaser = 0; numberSpawnerLaser < numberOfLasersToSpawn; ++numberSpawnerLaser)
            {
                yield return new WaitForSeconds(delayBetweenEachLaser);

                GameObject laser = Instantiate(lasePrefab, new Vector2(Random.Range(leftBorderSpawnLaser, rightBorderSpawnLaser), positionYLaser), Quaternion.identity);
                laser.GetComponent<PlasmaLaserController>().OnLaserFinish += LaserFinish;
            }
        }

        public void LaserFinish(PlasmaLaserController controller)
        {
            controller.OnLaserFinish -= LaserFinish;

            numberOfLaserFinish++;
            if (numberOfLaserFinish == numberOfLasersToSpawn)
                Finish();
        }
    }
}
