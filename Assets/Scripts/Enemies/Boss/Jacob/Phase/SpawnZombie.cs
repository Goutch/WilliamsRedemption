using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Jacob
{
    class SpawnZombie : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Jacob.IdlePhase1 + "' ")]
        [SerializeField] private Animator animator;
        [Tooltip("Require health component")]
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private float cooldown;


        private float lastTimeUsed;

        private SpawnedEnemyManager spawnedEnemyManager;

        private void Awake()
        {
            spawnedEnemyManager = GetComponent<SpawnedEnemyManager>();
        }

        public override void Act()
        {    

        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown && !spawnedEnemyManager.IsAllEnemySpawned())
                return true;
            else
                return false;
        }
        public override void Enter()
        {
            base.Enter();

            lastTimeUsed = Time.time;
            animator.SetTrigger(Values.AnimationParameters.Jacob.IdlePhase1);

            SpawnAZombie();
        }

        private void SpawnAZombie()
        {
            spawnedEnemyManager.SpawnEnemies(zombiePrefab, transform.position, Quaternion.identity);

        }
    }
}
