using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    public class PlasmaController : ProjectileController
    {
        [SerializeField] private float delayBeforeBulletCanKillHisShooter;

        private float bulletShotAt;

        protected new void Awake()
        {
            base.Awake();
            bulletShotAt = Time.time;
        }

        public bool CanBulletKillHisShooter()
        {
            return Time.time - bulletShotAt > delayBeforeBulletCanKillHisShooter;
        }
    }
}

