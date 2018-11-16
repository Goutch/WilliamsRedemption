using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Cannon : Enemy
    {
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
            GameObject projectile = Instantiate(bulletPrefab, projectileSpawnPoint.position, this.transform.rotation);
            projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Enemy);
            animator.SetTrigger("Shoot");
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


