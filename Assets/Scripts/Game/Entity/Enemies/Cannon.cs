using Game.Entity.Enemies.Attack;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Cannon : Enemy
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private float ProjectileLifeSpanInSeconds;
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
            Destroy(Instantiate(bulletPrefab, projectileSpawnPoint.position, this.transform.rotation),
                ProjectileLifeSpanInSeconds);
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

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type == HitStimulus.DamageType.Enemy)
                return false;
            return true;
        }
    }
}