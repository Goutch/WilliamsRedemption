using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class ShootLightWall : Capacity
    {
        [SerializeField] private float shieldCost;
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject projectileSpawnPoint;

        private ShieldManager shieldManager;
        private Animator animator;

        protected override void Init()
        {
            animator = GetComponent<Animator>();
        }

        public override void Enter()
        {
            base.Enter();

            shieldManager = GetComponent<ShieldManager>();

            animator.SetTrigger(Values.AnimationParameters.Jean.BothHandShoot);

            Instantiate(projectile, projectileSpawnPoint.transform.position, transform.rotation);

            shieldManager.UseShield(shieldCost);

            Finish();
        }

        public override void Act()
        {
        }

        public override bool CanEnter()
        {
            return true;
        }
    }
}