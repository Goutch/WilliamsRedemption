using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Playmode.EnnemyRework.Boss;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Jean
{
    class JeanPhase : SequentialPhaseWithWait
    {
        private RootMover mover;

        private void Awake()
        {
            mover = GetComponent<RootMover>();
        }

        public override void Act()
        {
            mover.LookAtPlayer();
            base.Act();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override bool CanEnter()
        {
            return true;
        }

        protected override void EnterIdle()
        {
            
        }
    }
}
