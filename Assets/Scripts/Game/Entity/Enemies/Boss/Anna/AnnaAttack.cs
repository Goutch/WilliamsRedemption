using Game.Entity.Player;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    class AnnaAttack : Capacity
    {
        [SerializeField] private float range;

        private Animator animator;
        private RootMover mover;

        protected override void Init()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<RootMover>();
        }

        public override void Act()
        {

        }

        public override bool CanEnter()
        {
            return Vector2.Distance(player.transform.position, transform.position) < range;
        }

        public override void Enter()
        {
            base.Enter();

            mover.LookAtPlayer();
            animator.SetTrigger(Values.AnimationParameters.Anna.Attack);
        }

        [UsedImplicitly]
        public void AttackFinish()
        {
            Finish();
        }
    }
}
