

using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Edgar
{
    public class EdgarController : BossController
    {
        protected override void OnHit(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper)
            {
                health.Hit();
                animator.SetTrigger(Values.AnimationParameters.Edgar.Hurt);
            }
        }
    }
}


