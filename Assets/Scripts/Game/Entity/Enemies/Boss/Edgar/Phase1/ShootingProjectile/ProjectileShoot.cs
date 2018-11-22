using Game.Entity.Player;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Edgar
{
    [RequireComponent(typeof(ProjectileManager))]
    class ProjectileShoot : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.PlasmaShoot + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [SerializeField] private GameObject bullet;
        [SerializeField] private GameObject particuleEffect;
        [SerializeField] private Transform spawnPoint;

        private ProjectileManager projectileManager;

        private float lastTimeCapacityUsed;

        protected override void Init()
        {
            if (capacityUsableAtStart)
                lastTimeCapacityUsed = -cooldown;

            projectileManager = GetComponent<ProjectileManager>();
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

            if(animator != null)
                animator.SetTrigger(Values.AnimationParameters.Edgar.PlasmaShoot);

            if(particuleEffect != null)
                Instantiate(particuleEffect, spawnPoint);
        }

        public void ShootPlasmaProjectile()
        {
            Quaternion direction = PlayerDirection();
            GameObject projectile = projectileManager.SpawnProjectile(bullet, spawnPoint.position, direction);

            Finish();
        }

        private Quaternion PlayerDirection()
        {
            Vector2 dir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }
    }
}
