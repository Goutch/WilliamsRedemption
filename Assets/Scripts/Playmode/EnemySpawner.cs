using System.Collections;
using System.Collections.Generic;
using Playmode.EnnemyRework;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private Enemy enemyStrategyToSpawn;
	private EnemyControllerRework spawnedEnnemy;

	private void SpawnEnemy()
	{
		spawnedEnnemy= Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyControllerRework>();
		spawnedEnnemy.Init(enemyStrategyToSpawn);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(spawnedEnnemy==null)
		SpawnEnemy();
	}
}
