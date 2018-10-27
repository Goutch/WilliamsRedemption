using Boss;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Jean
{
    class ShootLight : Capacity
    {
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [SerializeField] private float shieldCost;

        private ShieldManager shieldManager;

        private float lastTimeUsed;

        private void Awake()
        {

        }

        private void OnEnable()
        {
            shieldManager = GetComponentInChildren<ShieldManager>();

            if (capacityUsableAtStart)
                lastTimeUsed = -cooldown;
        }

        public override void Act()
        {

            Finish();
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();

            shieldManager = GetComponentInChildren<ShieldManager>();

            shieldManager.UseShield(shieldCost);

            lastTimeUsed = Time.time;

        }
    }
}
