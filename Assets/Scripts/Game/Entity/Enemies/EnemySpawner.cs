using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int spawnNumber;
        [SerializeField] private float secondsBetweenSpawns;
        [SerializeField] private bool spawnOneTime = true;
        private GameObject[] spawnedEnnemies;
        private bool spawning = false;

        private void Awake()
        {
            spawnedEnnemies = new GameObject[spawnNumber];
        }

        private void SpawnEnemies()
        {
            StartCoroutine(SpawnEnemyAfterSecondsRoutine(secondsBetweenSpawns));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!spawning)
                SpawnEnemies();
        }



        private IEnumerator SpawnEnemyAfterSecondsRoutine(float second)
        {
            spawning = true;
            for (int i = 0; i < spawnedEnnemies.Length; i++)
            {
                if (spawnedEnnemies[i] == null)
                {
                    spawnedEnnemies[i] = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(second);
                }
            }

            spawning = false;
            if (spawnOneTime)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

