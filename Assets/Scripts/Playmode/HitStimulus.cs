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
		if (other.gameObject.tag == "Player" && gameObject.tag == "Enemy")
		{
			hitSensor = other.gameObject.GetComponentInChildren<HitSensor>();
		}
		if(hitSensor != null)
		{
			hitSensor.Hit(hitPoints);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if ((other.gameObject.tag == "ProjectilePlayer" && gameObject.tag == "Enemy" )|| 
		    (other.gameObject.tag == "ProjectileEnemy" && gameObject.tag == "Player"))
		{
			hitSensor = gameObject.GetComponentInChildren<HitSensor>();
			Destroy(other.gameObject);
		}
		else if ((other.gameObject.tag == "Enemy" && gameObject.tag == "ProjectilePlayer") || 
		         (other.gameObject.tag == "Player" && gameObject.tag == "ProjectileEnemy"))
		{
			hitSensor = gameObject.GetComponentInChildren<HitSensor>();
			Destroy(gameObject);
		}
		if(hitSensor != null)
		{
			hitSensor.Hit(hitPoints);
		}
	}
}