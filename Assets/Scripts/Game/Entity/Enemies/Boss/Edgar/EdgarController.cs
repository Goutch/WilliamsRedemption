using Game.Entity;

namespace Game.Entity.Enemies.Boss.Edgar
{
    public class EdgarController : BossController
    {
        private RootMover mover;

        private void OnEnable()
        {
            mover = GetComponent<RootMover>();
            mover.LookAtPlayer();
        }

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


