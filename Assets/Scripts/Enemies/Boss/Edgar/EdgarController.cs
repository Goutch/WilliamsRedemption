using Boss;

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


