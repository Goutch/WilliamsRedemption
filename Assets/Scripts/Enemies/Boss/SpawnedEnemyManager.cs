using System.Collections.Generic;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss
{
    class SpawnedEnemyManager : MonoBehaviour
    {
        [SerializeField] private int numberOfEnemySpawnAllowed;

        public event HealthEventHandler OnEnnemyDied;

        private List<GameObject> enemies;

        private int numberOfEnemySpawn = 0;

        private void Awake()
        {
            enemies = new List<GameObject>();
        }

        public void SpawnEnemies(GameObject prefab, Vector3 position, Quaternion quaternion)
        {
            if(numberOfEnemySpawn < numberOfEnemySpawnAllowed)
            {
                GameObject enemy = Instantiate(prefab, position, quaternion);

                enemies.Add(enemy);
                enemy.GetComponent<Health>().OnDeath += SpawnedEnemyManager_OnDeath;

                numberOfEnemySpawn++;
            }
        }

        public bool IsAllEnemySpawned()
        {
            return numberOfEnemySpawn == numberOfEnemySpawnAllowed;
        }

        private void SpawnedEnemyManager_OnDeath(GameObject enemy)
        {
            enemies.Remove(enemy);

            OnEnnemyDied?.Invoke(enemy);
        }

        public int GetNumberOfEnemies()
        {
            return enemies.Count;
        }

        public void ResetEnemySpawnedCount()
        {
            enemies.Clear();
            numberOfEnemySpawn = 0;
        }
    }
}
