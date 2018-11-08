using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class ShootLightWall : Capacity
    {
        [SerializeField] private float shieldCost;
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject projectileSpawnPoint;
        [SerializeField] private float delayAfterCapacityUsed;

        private ShieldManager shieldManager;

        public override void Enter()
        {
            base.Enter();

            shieldManager = GetComponent<ShieldManager>();

            Instantiate(projectile, projectileSpawnPoint.transform.position, transform.rotation);

            shieldManager.UseShield(shieldCost);

            StartCoroutine(WaitBeforeFinish());
        }

        private IEnumerator WaitBeforeFinish()
        {
            yield return new WaitForSeconds(delayAfterCapacityUsed);
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
