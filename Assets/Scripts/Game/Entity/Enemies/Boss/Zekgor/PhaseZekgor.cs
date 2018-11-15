using Game.Entity.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Zekgor
{
    class PhaseZekgor : SequentialLoopPhase
    {
        private RootMover mover;

        protected override void Init()
        {
            mover = GetComponent<RootMover>();
        }

        protected override void Idle()
        {
            base.Idle();

            mover.LookAtPlayer();

            if(Mathf.Abs(player.transform.position.x - transform.position.x) > 0.03f)
                mover.MoveOnXAxis(transform.rotation.y == 1 ? -1 : 1);
        }
    }
}
