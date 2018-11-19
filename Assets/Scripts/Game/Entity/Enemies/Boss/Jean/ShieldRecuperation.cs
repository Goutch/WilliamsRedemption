using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Entity.Enemies.Boss.Jean
{
    class ShieldRecuperation : Vulnerable
    {
        private ShieldManager shieldManager;

        private void Awake()
        {
            shieldManager = GetComponent<ShieldManager>();
        }

        public override void Act()
        {
            base.Act();
        }

        public override void Finish()
        {
            base.Finish();
            shieldManager.ShieldPercent = 1;
            shieldManager.IsShieldActive = true;
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
