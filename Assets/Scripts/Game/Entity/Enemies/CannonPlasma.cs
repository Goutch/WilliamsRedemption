using Game.Entity.Enemies.Attack;

namespace Game.Entity.Enemies
{
    public class CannonPlasma : Cannon
    {

        protected override void OnHit(HitStimulus other)
        {
            if(other.GetComponent<PlasmaController>() != null)
                base.OnHit(other);
        }
    }
}


