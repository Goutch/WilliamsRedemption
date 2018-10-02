﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HitStimulus : MonoBehaviour
{
	private HitSensor hitSensor;
		
	private void OnCollisionEnter2D(Collision2D other)
	{
		hitSensor = null;
		if (other.collider.tag == "Player" && gameObject.tag == "Enemy")
		{
			hitSensor = other.gameObject.GetComponent<HitSensor>();
		}
		if(hitSensor != null)
		{
			hitSensor.Hit();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "ProjectilePlayer" && gameObject.tag == "Enemy" 
		    || other.tag == "ProjectileEnemy" && gameObject.tag == "Player")
		{
			gameObject.GetComponentInChildren<HitSensor>().Hit();
			Destroy(other.gameObject);
		}
		if (other.gameObject.GetComponent<PlasmaController>() != null)
		{ 
			if (other.tag == "ProjectileEnemy" && gameObject.tag == "ProjectilePlayer" 
			                                        && other.gameObject.GetComponent<ProjectileController>().CanBeReturned==true)
			{
				other.gameObject.GetComponent<PlasmaController>().ChangeDirection();
			}
			else if (other.gameObject.GetComponent<PlasmaController>().CanBulletKillHisShooter() == true
			         && other.tag == "ProjectileEnemy" && gameObject.tag == "Enemy")
			{
				gameObject.GetComponentInChildren<HitSensor>().Hit();
				Destroy(other.gameObject);
			}
		}
	}
}
