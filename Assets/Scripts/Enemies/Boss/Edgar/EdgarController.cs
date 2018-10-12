using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;
using UnityEngine.Tilemaps;

namespace Edgar
{
    public class EdgarController : BossController
    {
        protected override void HandleCollision(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper)
                health.Hit();
        }
    }
}


