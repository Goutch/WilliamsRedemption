using Game.Entity.Enemies.Attack;
using Game.Entity.Player;
using Harmony;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Zombie : WalkTowardPlayerEnnemy
    {
        [SerializeField] private Vector2 bulletKnockBackForce;
        [SerializeField] private Vector2 playerKnockBackForce;

        private new Rigidbody2D rigidbody;
        private bool knocked = false;

        protected override void Init()
        {
            base.Init();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private new void FixedUpdate()
        {
            if (!knocked)
                base.FixedUpdate();
            if (knocked && rigidbody.velocity.y == 0)
                    knocked = false;
        }

        protected override void OnHit(HitStimulus hitStimulus)
        {
            if(hitStimulus.Type == HitStimulus.DamageType.Darkness)
            {
                base.OnHit(hitStimulus);
            }
            else
            {
                rootMover.LookAtPlayer();
                int direction = (transform.rotation.y == 1 ? -1 : 1);

                rigidbody.AddForce(new Vector2(bulletKnockBackForce.x * -direction, bulletKnockBackForce.y),
                    ForceMode2D.Impulse);
                knocked = true;
            }
        }
    }
}