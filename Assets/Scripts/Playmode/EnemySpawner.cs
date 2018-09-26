using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	[SerializeField] private GameObject enemyPrefab;
	
	void Awake ()
	{
		SpawnEnemy();
	}

	private void SpawnEnemy()
	{
		Instantiate(enemyPrefab, transform.position, Quaternion.identity);
	}
}
