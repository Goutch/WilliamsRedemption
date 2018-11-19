using Game.Puzzle;
using Game.Puzzle.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    [RequireComponent(typeof(SpawnedEnemyManager))]
    class DeathPhase2 : NonSequentialPhase
    {
        [SerializeField] private GameObject cloneToSpawn;
        [SerializeField] private Transform[] positionsToSpawn;
        [SerializeField] private Transform positionToGoWhenPhaseStart;

        [SerializeField] private SpawnedEnemyManager enemyManager;
        [SerializeField] private new MeshLight light;

        private void Awake()
        {
            enemyManager.NumberOfEnemySpawnAllowed = positionsToSpawn.Length;
        }

        public override bool CanEnter()
        {
            return true;
        }

        public override void Enter()
        {
            base.Enter();

            transform.position = positionToGoWhenPhaseStart.position;

            light.Open();

            foreach (Transform positionToSpawn in positionsToSpawn)
            {
                enemyManager.SpawnEnemy(cloneToSpawn, positionToSpawn.position, Quaternion.identity);
            }

            enemyManager.OnEnnemyDied += EnemyManager_OnEnnemyDied;
        }

        private void EnemyManager_OnEnnemyDied(GameObject gameObject)
        {
            if(enemyManager.GetNumberOfEnemies() == 0 && enemyManager.IsAllEnemySpawned())
            {
                enemyManager.OnEnnemyDied -= EnemyManager_OnEnnemyDied;

                Finish();
            }
        }
    }
}
