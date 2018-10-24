using System.Collections;
using System.Collections.Generic;
using Playmode.EnnemyRework;
using UnityEngine;

public class Cannon : Enemy
{
	[SerializeField] private int rotationCannon;
	[SerializeField] private GameObject bulletPrefab;
	private float timeJustAfterShooting;
	private const float TIME_BEFORE_SHOOTING_AGAIN=2;

	protected override void Init()
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
		GameObject projectile=Instantiate(bulletPrefab,transform.position,Quaternion.AngleAxis(rotationCannon,Vector3.back));	
		projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Enemy);
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
