using Game.Entity.Enemies.Attack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Entity.Enemies.Boss.Zekgor
{
    public class ZekgorController : BossController
    {
        protected override bool OnHit(HitStimulus other)
        {
            if (other.Type == HitStimulus.DamageType.Darkness&&!IsInvulnerable)
            {
                base.OnHit(other);
                return true;
            }

            return false;
        }
    }
}