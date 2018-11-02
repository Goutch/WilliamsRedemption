using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Entity.Enemies.Boss.Jean
{
    class ShieldRecuperation : Capacity
    {
        private ShieldManager shieldManager;

        private void Awake()
        {
            shieldManager = GetComponent<ShieldManager>();
        }

        public override void Act()
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            shieldManager.ShieldPercent = 1;
            shieldManager.IsShieldActive = true;
            Finish();
        }

        public override bool CanEnter()
        {
            if (shieldManager.ShieldPercent == 0)
                return true;
            else
                return false;
        }
    }
}
