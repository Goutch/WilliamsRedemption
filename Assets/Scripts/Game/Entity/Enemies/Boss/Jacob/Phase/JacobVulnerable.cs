using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jacob
{
    [RequireComponent(typeof(Health))]
    class JacobVulnerable : Vulnerable
    {
        private Health health;
        private SpawnedEnemyManager spawnedEnemyManager;

        private bool canEnter = false;

        public override bool CanEnter()
        {
            return canEnter;
        }

        public override void Finish()
        {
            spawnedEnemyManager.ResetEnemySpawnedCount();

            canEnter = false;
            base.Finish();
        }

        private void Awake()
        {
            health = GetComponent<Health>();
            spawnedEnemyManager = GetComponent<SpawnedEnemyManager>();
        }

        private void OnEnable()
        {
            health.OnHealthChange += Health_OnHealthChange;
            spawnedEnemyManager.OnEnnemyDied += Zombie_OnDeath;
        }

        private void OnDisable()
        {
            health.OnHealthChange -= Health_OnHealthChange;
            spawnedEnemyManager.OnEnnemyDied -= Zombie_OnDeath;
        }


        private void Health_OnHealthChange(GameObject gameObject)
        {
            Finish();
        }

        private void Zombie_OnDeath(GameObject gameObject)
        {
            if (spawnedEnemyManager.IsAllEnemySpawned() && spawnedEnemyManager.GetNumberOfEnemies() == 0)
            {
                canEnter = true;
            }
        }
    }
}
