using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Entity.Enemies.Boss.Zekgor
{
    public class ZekgorController : BossController
    {
        protected override void OnHit(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper&&!IsInvulnerable)
            {
                base.OnHit(other);
            }
        }
    }
}