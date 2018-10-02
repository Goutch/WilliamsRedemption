using System.CodeDom;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public class Zombie : WalkTowardPlayerEnnemy
    {
        [SerializeField] private Vector2 bulletKnockBackForce;
        private Rigidbody2D rigidbody;
        private bool knocked = false;

        protected override void Init()
        {
            base.Init();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!knocked)
                base.FixedUpdate();
            if(knocked)
                if (rigidbody.velocity.y == 0)
                    knocked = false;
        }

        public override void ReceiveDamage(Collider2D other)
        {
            if (PlayerController.instance.CurrentController is ReaperController)
            {
                base.ReceiveDamage(other);
            }
            else if (PlayerController.instance.CurrentController is WilliamController)
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