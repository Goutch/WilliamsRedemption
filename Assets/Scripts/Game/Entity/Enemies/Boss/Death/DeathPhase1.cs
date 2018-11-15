using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    class DeathPhase1 : NonSequentialPhase
    {
        private Animator animator;
        [Range(0,1)][SerializeField] private float percentageHealthTransitionCondition;
        [SerializeField] private SpawnedEnemyManager enemyManager;
        private Health health;

        protected override void Init()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            health.OnHealthChange += Health_OnHealthChange;
        }

        public override void Finish()
        {
            base.Finish();

            enemyManager.Clear();
        }

        private void Health_OnHealthChange(GameObject gameObject)
        {
            if (health.HealthPoints / (float)health.MaxHealth <= percentageHealthTransitionCondition)
            {
                health.OnHealthChange -= Health_OnHealthChange;
                Finish();
            }
        }

        protected override void EnterIdle()
        {
            base.EnterIdle();

            animator.SetTrigger(Values.AnimationParameters.Death.Idle);
        }

        public override bool CanEnter()
        {
            return true;
        }
    }
}
