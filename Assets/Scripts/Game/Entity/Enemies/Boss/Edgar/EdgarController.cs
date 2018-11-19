using Game.Entity;
using Game.Entity.Enemies.Attack;

namespace Game.Entity.Enemies.Boss.Edgar
{
    public class EdgarController : BossController
    {
        private RootMover mover;

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if(hitStimulus.Type == HitStimulus.DamageType.Darkness)
            {
                health.Hit(hitStimulus.gameObject);
                return true;
            } 
            else if(hitStimulus.Type == HitStimulus.DamageType.Physical)
            {
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            mover = GetComponent<RootMover>();
            mover.LookAtPlayer();
        }
    }
}


