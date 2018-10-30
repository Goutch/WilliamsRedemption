using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    class ProjectileShoot : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.PlasmaShoot + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [SerializeField] private GameObject bullet;
        [SerializeField] private GameObject particuleEffect;
        [SerializeField] private Transform spawnPoint;

        private float lastTimeCapacityUsed;

        private void Awake()
        {
            if (capacityUsableAtStart)
                lastTimeCapacityUsed = -cooldown;
        }

        public override void Act()
        {

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

            lastTimeCapacityUsed = Time.time;

            animator.SetTrigger(Values.AnimationParameters.Edgar.PlasmaShoot);

            Instantiate(particuleEffect, spawnPoint);
        }

        public void ShootPlasmaProjectile()
        {
            Quaternion direction = PlayerDirection();
            GameObject projectile = Instantiate(bullet, spawnPoint.position, direction);
            projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Enemy);

            Finish();
        }

        private Quaternion PlayerDirection()
        {
            Vector2 dir = PlayerController.instance.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }
    }
}
