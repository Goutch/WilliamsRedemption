using System.Collections;
using Harmony;
using UnityEngine;

namespace Game.Puzzle
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] protected GameObject enemyPrefab;

        protected GameObject spawnedEnemy;
        protected bool spawned = false;

        protected virtual void SpawnEnemy()
        {
            spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

            spawned = true;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.Root().CompareTag(Values.Tags.Player))
            {
                if (!spawned)
                    SpawnEnemy();
            }
        }
    }
}