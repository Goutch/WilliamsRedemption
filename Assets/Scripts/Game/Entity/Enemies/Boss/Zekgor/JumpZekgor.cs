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
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;

        [SerializeField] private GameObject landingEffect;
        [SerializeField] private Transform landingEffectSpawnPoint;

        private RootMover mover;
        private Rigidbody2D rb;

        private float lastTimeCapacityUsed;

        protected override void Init()
        {
            mover = GetComponent<RootMover>();
            rb = GetComponent<Rigidbody2D>();

            if (capacityUsableAtStart)
                lastTimeCapacityUsed = -cooldown;
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

            lastTimeCapacityUsed = Time.time;

            mover.Jump();
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeCapacityUsed > cooldown)
                return true;
            else
                return false;
        }
    }
}
