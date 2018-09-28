using UnityEngine;

namespace Playmode.EnnemyRework
{
    public class Zombie : WalkTowardPlayerEnnemy
    {
        [SerializeField] private Vector2 bulletKnockBackForce;
        private Rigidbody2D rigidbody;
        
        protected override void Init()
        {
            base.Init();
            rigidbody=GetComponent<Rigidbody2D>();
        }

        public override void ReceiveDamage()
        {
            if (PlayerController.instance.CurrentController is ReaperController)
            {
                base.ReceiveDamage();
            }


            else if (PlayerController.instance.CurrentController is WilliamController)
            {
                rigidbody.AddForce(bulletKnockBackForce,ForceMode2D.Impulse);
            }
        }
    }
}