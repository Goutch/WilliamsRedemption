using Boss;
using System.Collections;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Jean
{
    class ShootLightWall : Capacity
    {
        [SerializeField] private float cooldown;
        [SerializeField] private float shieldCost;
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject projectileSpawnPoint;
        [SerializeField] private float delayAfterCapacityUsed;

        private ShieldManager shieldManager;
        private bool canEnterAsked1Time = false;

        private float lastTimeUsed;

        public override void Finish()
        {
            canEnterAsked1Time = false;

            base.Finish();
        }

        public override bool CanEnter()
        {
            if(!canEnterAsked1Time)
            {
                lastTimeUsed = Time.time;
                canEnterAsked1Time = true;
            }

            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();

            shieldManager = GetComponentInChildren<ShieldManager>();

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
    }
}
