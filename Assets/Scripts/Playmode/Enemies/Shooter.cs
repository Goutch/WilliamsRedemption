using System.Collections;
using System.Collections.Generic;
using Playmode.EnnemyRework;
using UnityEngine;

public class Shooter : WalkTowardPlayerEnnemy {

	[SerializeField] private GameObject bulletPrefab;
	private float timeJustAfterShooting;
	private const float TIME_BEFORE_SHOOTING_AGAIN=2;
	
	protected override void FixedUpdate()
	{
		if(CanShoot())
		{
			Shoot();
		}
		base.FixedUpdate();
	}
	private bool CanShoot()
	{
		if (Time.time-timeJustAfterShooting>TIME_BEFORE_SHOOTING_AGAIN)
		{
			timeJustAfterShooting = Time.time;
			return true;
		}
		return false;
	}
	private void Shoot()
	{
		int temporarydirection=ChooseAngleToShoot();
		
		GameObject projectile=Instantiate(bulletPrefab,transform.position,Quaternion.AngleAxis(temporarydirection,Vector3.back));
		projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Ennemy);
	}

	private int ChooseAngleToShoot()
	{
		if (currenDirection == -1)
		{
			return 180;
		}
		return 0;
	}
}
