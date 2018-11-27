using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class RecuperationPhase : SequentialPhase
    {
        [SerializeField] private float inactiveTimeBeforeUseCapacity;

        private float lastTimeUsed;
        protected override void Init()
        {
            
        }

        public override void Act()
        {
            if(Time.time - lastTimeUsed > inactiveTimeBeforeUseCapacity)
                base.Act();
        }

        public override void Enter()
        {
            base.Enter();
            lastTimeUsed = Time.time;
        }
    }
}
