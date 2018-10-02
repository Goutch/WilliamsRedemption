using System.Collections;
using System.Collections.Generic;
using Playmode.EnnemyRework;
using UnityEngine;

public class Sorcerer : Enemy
{

	[SerializeField] private int numberOfBatsToSummon;
	[SerializeField] private GameObject batPrefab;
	
	private List<GameObject> bats;
	private float timeJustAfterSpawningBats;
	private const int TIME_BEFORE_SPAWNING_BATS_AGAIN=5;
	private const int TIME_BETWEEN_SPAWNED_BATS=1;
	private bool didBatsJustDie;
	
	protected override void Init()
	{
		bats=new List<GameObject>();
		timeJustAfterSpawningBats = 0;
	}

	protected override void HandleCollision(HitStimulus other)
	{
		GetComponent<Health>().Hit();
	}

	public void Update()
	{
		if (IsNoBatRemaining() && IsItTimeToRespawnBats())
		{
			StartCoroutine(SummonBats());
			didBatsJustDie = true;
		}
	}

	private IEnumerator SummonBats()
	{
		for (int i = 0; i < numberOfBatsToSummon; ++i)
		{
			GameObject spawnedObject = Instantiate(batPrefab, new Vector3(transform.position.x - 1
				, transform.position.y, transform.position.z), Quaternion.identity);
			spawnedObject.GetComponent<Health>().OnDeath+=StartTimerForRespawn;
			bats.Add(spawnedObject);
			yield return new WaitForSeconds(TIME_BETWEEN_SPAWNED_BATS);
		}
	}

	private bool IsNoBatRemaining()
	{
		if (bats.Count == 0)
		{
			return true;
		}

		return false;
	}

	private void StartTimerForRespawn(GameObject bat)
	{
		bats.Remove(bat);
		if (IsNoBatRemaining())
		{
			timeJustAfterSpawningBats = Time.time;
		}
	}

	private bool IsItTimeToRespawnBats()
	{
		return Time.time - timeJustAfterSpawningBats >= TIME_BEFORE_SPAWNING_BATS_AGAIN;
	}
}
