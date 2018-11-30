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
        [Range(0, 1)] [SerializeField] private float percentageHealthTransitionCondition;
        //BEN_REVIEW : J'ai pas été capable de comprendre pourquoi tu en avais plusieurs sur le même GameObject.
        [SerializeField] private SpawnedEnemyManager[] enemyManagers;

        private Health health;
        private Animator animator;

        protected override void Init()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            enemyManagers = GetComponents<SpawnedEnemyManager>();
            health.OnHealthChange += Health_OnHealthChange;
        }

        public override void Finish()
        {
            foreach (SpawnedEnemyManager spawnedEnemyManager in enemyManagers)
                spawnedEnemyManager.Clear();

            base.Finish();
        }

        //BEN_CORRECTION : Nommage. Tu as utilisé le nom proposé par Visual Studio sans te poser de quesiton ?
        private void Health_OnHealthChange(GameObject receiver, GameObject attacker)
        {
            if (health.HealthPoints / (float) health.MaxHealth <= percentageHealthTransitionCondition)
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