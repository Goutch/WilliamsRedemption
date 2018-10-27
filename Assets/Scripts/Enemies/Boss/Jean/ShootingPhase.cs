using UnityEngine;
using Boss;

namespace Playmode.EnnemyRework.Boss.Jean
{
    class ShootingPhase : SequentialPhaseWithWait
    {
        private ShieldManager shieldManager;

        public override bool CanEnter()
        {
            return base.CanEnter() && shieldManager.ShieldPercent > 0;
        }

        private void Awake()
        {
            shieldManager = GetComponentInChildren<ShieldManager>();
        }

        public override void Act()
        {
            if (shieldManager.ShieldPercent == 0)
                Finish();

            base.Act();
        }

        protected override void EnterIdle()
        {
            
        }
    }
}
