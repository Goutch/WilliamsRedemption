using Game.Entity.Player;
using Harmony;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Zombie : WalkTowardPlayerEnnemy
    {
        [SerializeField] private Vector2 bulletKnockBackForce;
        [SerializeField] private Vector2 playerKnockBackForce;

        private Rigidbody2D rigidbody;
        private bool knocked = false;

        protected override void Init()
        {
            base.Init();
            rigidbody = GetComponent<Rigidbody2D>();
            GetComponent<HitStimulus>().OnHitOther += OnHitOther;
        }

        private void FixedUpdate()
        {
            if (!knocked)
                base.FixedUpdate();
            if (knocked)
                if (rigidbody.velocity.y == 0)
                    knocked = false;
        }

        private void OnHitOther(HitSensor other)
        {
            if (other.CompareTag(Values.Tags.Player))
            {
                other.Root().GetComponent<KinematicRigidbody2D>().AddForce(new Vector2(playerKnockBackForce.x*currenDirection,playerKnockBackForce.y));
            }
        }
        protected override void OnHit(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper)
            {
                base.OnHit(other);
            }
            else if (other.DamageSource == HitStimulus.DamageSourceType.William)
            {
                int dir;
                if (other.transform.position.x < this.transform.position.x)
                {
                    dir = 1;
                }
                else
                {
                    dir = -1;
                }

                rigidbody.AddForce(new Vector2(bulletKnockBackForce.x * dir, bulletKnockBackForce.y),
                    ForceMode2D.Impulse);
                knocked = true;
            }      
        }
    }
}