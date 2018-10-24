using System.Collections;
using System.Collections.Generic;
using Playmode.EnnemyRework;
using UnityEngine;

public class Shooter : WalkTowardPlayerEnnemy {

	[Header("Data only for Shooter")][SerializeField] private GameObject bulletPrefab;
	[SerializeField] private int distanceLimitBetweenPlayerAndShooter;
	[SerializeField] private int timeBeforeShootingAgain;
	
	private float timeJustAfterShooting;
	private float distanceBetweenShooterAndPlayer;
	
	protected override void FixedUpdate()
	{
		UpdateShoot();
		UpdateMovement();	
	}

	private void UpdateShoot()
	{
		if(CanShoot())
		{
			Shoot();
		}
	}

	private void UpdateMovement()
	{
		distanceBetweenShooterAndPlayer = Mathf.Abs(PlayerController.instance.transform.position.x - transform.position.x);
		if (distanceBetweenShooterAndPlayer >= distanceLimitBetweenPlayerAndShooter)
		{
			base.FixedUpdate();
		}
		else
		{
			rootMover.WalkToward(0);
		}
	}
	
	private bool CanShoot()
	{
		if (Time.time-timeJustAfterShooting>timeBeforeShootingAgain)
		{		
			return true;
		}
		return false;
	}
	
	private void Shoot()
	{
		int directionToShoot=ChooseAngleToShoot();	
		GameObject projectile=Instantiate(bulletPrefab,transform.position,Quaternion.AngleAxis(directionToShoot,Vector3.back));
		projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Enemy);
		UpdateTimerToShoot();
	}

	private void UpdateTimerToShoot()
	{
		timeJustAfterShooting = Time.time;
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
