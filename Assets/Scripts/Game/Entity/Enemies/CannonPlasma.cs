using Game.Entity.Enemies.Attack;

namespace Game.Entity.Enemies
{
    public class CannonPlasma : Cannon
    {
        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type == HitStimulus.DamageType.Darkness &&
                hitStimulus.Range == HitStimulus.AttackRange.Ranger)
            {
                health.Hit(hitStimulus.gameObject);
                return true;
            }

            return false;

        }
    }
}


