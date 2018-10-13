using Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using Harmony;

namespace Edgar
{
    class PowerRelease : Capacity
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.PlasmaRelease + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject laserSpawnPointsZone;
        [SerializeField] private GameObject lasePrefab;
        [SerializeField] private float delayBetweenEachLaser;
        [SerializeField] private int numberOfLasersToSpawn;
        private int numberOfLaserFinish;

        private float rangeLaserSpawnLeftBorder;
        private float rangeLaserSpawnRightBorder;
        private float laserSpawnPointPositionY;

        private void Awake()
        {
            Vector2 size = laserSpawnPointsZone.GetComponent<Collider2D>().bounds.size;
            Vector2 center = laserSpawnPointsZone.GetComponent<Collider2D>().bounds.center;

            rangeLaserSpawnLeftBorder = -size.x/2 + center.x;
            rangeLaserSpawnRightBorder = size.x/2 + center.x;

            laserSpawnPointPositionY = center.y;
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
                GameObject laser = Instantiate(lasePrefab, new Vector2(UnityEngine.Random.Range(rangeLaserSpawnLeftBorder, rangeLaserSpawnRightBorder), laserSpawnPointPositionY), Quaternion.identity);
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
