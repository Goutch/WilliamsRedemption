using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    class ShadowSlashCombo : SequentialPhase
    {
        [SerializeField] private float cooldown;

        private Animator animator;

        private float lastTimeUsed;

        protected override void Init()
        {
            animator = GetComponent<Animator>();
            lastTimeUsed = Time.time;
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        protected override void EnterIdle()
        {
            base.EnterIdle();

            animator.SetTrigger(Values.AnimationParameters.Death.Idle);
        }


        public override void Finish()
        {
            base.Finish();
            lastTimeUsed = Time.time;
        }
    }
}