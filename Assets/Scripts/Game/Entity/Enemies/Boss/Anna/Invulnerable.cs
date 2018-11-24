using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    [RequireComponent(typeof(BossController), typeof(SpriteRenderer))]
    class Invulnerable : Capacity
    {
        [SerializeField] private float cooldown;
        [SerializeField] private float duration;

        private BossController bossController;
        private Animator animator;

        private float lastTimeUsed;
        private bool isCapacityBeingUsed = false;

        protected override void Init()
        {
            bossController = GetComponent<BossController>();
            animator = GetComponent<Animator>();
        }

        public override void Act()
        {
        }

        public override bool CanEnter()
        {
            return Time.time - lastTimeUsed > cooldown;
        }

        public override void Enter()
        {
            base.Enter();
            StopAllCoroutines();

            animator.SetTrigger(Values.AnimationParameters.Anna.CastInvulnerability);
            animator.SetBool(Values.AnimationParameters.Anna.Invulnerable, true);

            bossController.IsInvulnerable = true;
            isCapacityBeingUsed = true;
        }

        [UsedImplicitly]
        public void MakeInvulnerable()
        {
            Finish();

            animator.SetBool(Values.AnimationParameters.Anna.Invulnerable, true);
            bossController.IsInvulnerable = true;
        }

        public override void Finish()
        {
            base.Finish();

            lastTimeUsed = Time.time;

            StopAllCoroutines();
            StartCoroutine(RemoveInvulnerablityCoroutine());

            isCapacityBeingUsed = false;
        }

        private IEnumerator RemoveInvulnerablityCoroutine()
        {
            yield return new WaitForSeconds(duration);
            RemoveInvulnerablity();
        }

        private void RemoveInvulnerablity()
        {
            bossController.IsInvulnerable = false;
            animator.SetBool(Values.AnimationParameters.Anna.Invulnerable, false);
        }
    }
}