using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Cannon : Enemy
    {
        [SerializeField] private int rotationCannon;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        private float timeJustAfterShooting;
        private const float TIME_BEFORE_SHOOTING_AGAIN = 2;

        protected override void Init()
        {
            ResetTimeToShoot();
        }

        public void Update()
        {
            if (CanCannonShoot() == true)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            GameObject projectile = Instantiate(bulletPrefab, projectileSpawnPoint.position, Quaternion.AngleAxis(rotationCannon, Vector3.back));
        }

        private void ResetTimeToShoot()
        {
            timeJustAfterShooting = Time.time;
        }

        private bool CanCannonShoot()
        {
            if (Time.time - timeJustAfterShooting > TIME_BEFORE_SHOOTING_AGAIN)
            {
                timeJustAfterShooting = Time.time;
                return true;
            }
            return false;
        }

    }
}


