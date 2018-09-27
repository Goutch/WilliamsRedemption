using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStimulus : MonoBehaviour
{

	[SerializeField] private int hitPoints = 1;
	private HitSensor hitSensor;
	
	public int HitPoints
	{
		get { return hitPoints;}
		set { hitPoints=value; }
	}
	
	private void Awake()
	{
		ValidateSerializeFields();
	
	}
	
	private void ValidateSerializeFields()
	{
		if (hitPoints < 0)
			throw new ArgumentException("Hit points can't be less than 0.");
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		hitSensor = null;
		if (other.collider.tag == "Player" && gameObject.tag == "Enemy")
		{
			hitSensor = other.gameObject.GetComponent<HitSensor>();
			if(hitSensor != null)
			{
				hitSensor.Hit(hitPoints);
			}
		}
		else if (other.collider.tag == "ProjectilePlayer" && gameObject.tag == "Enemy")
		{
			hitSensor = gameObject.GetComponentInChildren<HitSensor>();
		}
		else if (other.collider.tag == "ProjectileEnemy" && gameObject.tag == "Player")
		{
			hitSensor = gameObject.GetComponentInChildren<HitSensor>();
		}
		if(hitSensor != null)
		{
			hitSensor.Hit(hitPoints);
		}
	}
}
