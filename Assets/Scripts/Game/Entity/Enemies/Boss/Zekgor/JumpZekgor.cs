using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Zekgor
{
    class JumpZekgor : Capacity
    {
        [SerializeField] private GameObject landingEffect;
        [SerializeField] private Transform landingEffectSpawnPoint;

        private RootMover mover;
        private Rigidbody2D rb;
        private Animator animator;

        private void Awake()
        {
           
            mover = GetComponent<RootMover>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
           
        }

        public override void Act()
        {
            if (rb.velocity.y == 0)
            {
                Finish();
            }
        }

        public override void Finish()
        {
            base.Finish();
            
            if(landingEffect != null && landingEffectSpawnPoint != null)
                Instantiate(landingEffect, landingEffectSpawnPoint.position, Quaternion.identity);
        }

        public override void Enter()
        {
            base.Enter();
            

            mover.Jump();
        }

        public override bool CanEnter()
        {
            return true;
        }

        
    }
}
