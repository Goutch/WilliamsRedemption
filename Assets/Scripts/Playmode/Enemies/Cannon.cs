using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : EnemyController
{
	[SerializeField] private int rotationCannon;
	[SerializeField] private GameObject bulletPrefab;
	private float timeJustAfterShooting;
	private const float TIME_BEFORE_SHOOTING_AGAIN=4;

	private void Awake()
	{
		ResetTimeToShoot();
	}

	public void Update()
	{
		if (CanCannonShoot() == true)
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		Instantiate(bulletPrefab,transform.position,Quaternion.identity/*AngleAxis(rotationCannon,Vector3.back)*/);	
	}

	private void ResetTimeToShoot()
	{
		timeJustAfterShooting = Time.time;
	}

	private bool CanCannonShoot()
	{
		if (Time.time-timeJustAfterShooting>TIME_BEFORE_SHOOTING_AGAIN)
		{
			timeJustAfterShooting = Time.time;
			return true;
		}
		return false;
	}
}
