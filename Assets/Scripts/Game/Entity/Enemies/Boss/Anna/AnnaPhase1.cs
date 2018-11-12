using Game.Entity.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    [RequireComponent(typeof(RootMover))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(BossController))]
    class AnnaPhase1 : NonSequentialPhase
    {
        [SerializeField] private float percentageHealthTransitionCondition;

        private RootMover mover;
        private Animator animator;
        private Health health;
        private BossController bossController;

        private void Awake()
        {
            mover = GetComponent<RootMover>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            bossController = GetComponent<BossController>();

            health.OnHealthChange += Health_OnHealthChange;
        }

        private void Health_OnHealthChange(GameObject gameObject)
        {
            if (health.HealthPoints / (float)health.MaxHealth <= percentageHealthTransitionCondition)
            {
                health.OnHealthChange -= Health_OnHealthChange;
                Finish();
            }
        }

        public override void Finish()
        {
            base.Finish();

            animator.SetBool(Values.AnimationParameters.Anna.Invulnerable, false);

            bossController.IsInvulnerable = false;
        }

        protected override void EnterIdle()
        {
            base.EnterIdle();
            animator.SetTrigger(Values.AnimationParameters.Anna.Walk);
        }

        protected override void Idle()
        {
            base.Idle();
            mover.LookAtPlayer();

            if (Mathf.Abs(transform.position.x - PlayerController.instance.transform.position.x) > 0.25f)
            {
                mover.MoveForward();

                animator.SetBool(Values.AnimationParameters.Anna.Walking, true);
            }
            else
            {
                animator.SetBool(Values.AnimationParameters.Anna.Walking, false);
            }

        }

        public override bool CanEnter()
        {
            return true;
        }
    }
}
