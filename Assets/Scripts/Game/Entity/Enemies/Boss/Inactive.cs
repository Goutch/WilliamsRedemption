using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    class Inactive : Capacity
    {
        [SerializeField] private float timeInactive;

        private float timeEntered;

        public override void Enter()
        {
            base.Enter();
            timeEntered = Time.time;
        }

        public override void Act()
        {
            if (Time.time - timeEntered > timeInactive)
                Finish();
        }

        public override bool CanEnter()
        {
            return true;
        }

        protected override void Init()
        {
           
        }
    }
}
