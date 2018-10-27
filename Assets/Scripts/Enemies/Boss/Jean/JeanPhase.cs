using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Playmode.EnnemyRework.Boss;

namespace Assets.Scripts.Enemies.Boss.Jean
{
    class JeanPhase : NonSequentialPhase
    {
        public override bool CanEnter()
        {
            return true;
        }

        protected override void EnterIdle()
        {
            
        }
    }
}
