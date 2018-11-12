using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    class SpotSpawnEnemy : Capacity
    {
        [SerializeField] private GameObject enemyToSpawn;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private float spawnDelay;
        [SerializeField] private SpawnedEnemyManager enemyManager;
        private float timeEnemyDied;

        private void OnEnable()
        {
            enemyManager.OnEnnemyDied += EnemyManager_OnEnnemyDied;
        }

        private void OnDisable()
        {
            enemyManager.OnEnnemyDied -= EnemyManager_OnEnnemyDied;
        }

        private void EnemyManager_OnEnnemyDied(GameObject gameObject)
        {
            timeEnemyDied = Time.time;
        }

        public override void Act()
        {

        }

        public override bool CanEnter()
        {
            return enemyManager.CanSpawnEnemy() && Time.time - timeEnemyDied > spawnDelay;
        }

        public override void Enter()
        {
            base.Enter();

            enemyManager.SpawnEnemy(enemyToSpawn, spawnPoint.position, Quaternion.identity);
        }
    }
}
