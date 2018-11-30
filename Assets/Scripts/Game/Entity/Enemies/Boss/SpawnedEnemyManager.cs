using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    class SpawnedEnemyManager : MonoBehaviour
    {
        [SerializeField] private int numberOfEnemySpawnAllowed;
        [SerializeField] private bool resetWhenAllEnemyDied = true;

        public event HealthEventHandler OnEnnemyDied;

        private List<GameObject> enemies;

        private int numberOfEnemySpawn = 0;

        public int NumberOfEnemySpawnAllowed
        {
            get { return numberOfEnemySpawnAllowed; }

            set { numberOfEnemySpawnAllowed = value; }
        }

        private void Awake()
        {
            enemies = new List<GameObject>();
        }

        public void SpawnEnemy(GameObject prefab, Vector3 position, Quaternion quaternion)
        {
            if (numberOfEnemySpawn < NumberOfEnemySpawnAllowed)
            {
                GameObject enemy = Instantiate(prefab, position, quaternion);

                enemies.Add(enemy);
                enemy.GetComponent<Health>().OnDeath += SpawnedEnemyManager_OnDeath;

                numberOfEnemySpawn++;
            }
        }

        public bool CanSpawnEnemy()
        {
            return numberOfEnemySpawn < NumberOfEnemySpawnAllowed;
        }

        public bool IsAllEnemySpawned()
        {
            return numberOfEnemySpawn == NumberOfEnemySpawnAllowed;
        }

        //BEN_CORRECTION : Nommage.
        private void SpawnedEnemyManager_OnDeath(GameObject receiver, GameObject attacker)
        {
            enemies.Remove(receiver);
            OnEnnemyDied?.Invoke(receiver, attacker);

            if (IsAllEnemySpawned() && GetNumberOfEnemies() == 0 && resetWhenAllEnemyDied)
                ResetEnemySpawnedCount();
        }

        //BEN_REVIEW : Aurait pu être une propriété.
        public int GetNumberOfEnemies()
        {
            return enemies.Count;
        }

        public void ResetEnemySpawnedCount()
        {
            Clear();
            numberOfEnemySpawn = 0;
        }

        public void Clear()
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                    Destroy(enemy);
            }

            enemies.Clear();
        }
    }
}