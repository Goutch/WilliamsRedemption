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
        }

        private void FixedUpdate()
        {
            if (!knocked)
                base.FixedUpdate();
            if (knocked)
                if (rigidbody.velocity.y == 0)
                    knocked = false;
        }

      
    }
}