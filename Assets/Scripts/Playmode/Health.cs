using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

	[SerializeField] private int healthPoints = 100;

	public int HealthPoints
	{
		get { return healthPoints; }
		private set { healthPoints = value; }
	}

	void Awake () {
		ValidateSerialisedFields();
	}

	private void ValidateSerialisedFields()
	{
		if (healthPoints < 0)
			throw new ArgumentException("HealthPoints can't be lower than 0.");
	}
	
	public void Hit(int hitPoints)
	{
		HealthPoints -= hitPoints;
	}
}
