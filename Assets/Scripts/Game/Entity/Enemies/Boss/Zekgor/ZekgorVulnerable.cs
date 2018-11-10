using Game.Entity;
using Game.Entity.Enemies.Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Entity.Enemies.Boss.Zekgor
{
    class ZekgorVulnerable : Vulnerable
    {
        public override bool CanEnter()
        {
            return base.CanEnter();
        }
    }
}
