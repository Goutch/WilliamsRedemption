using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : EnemyController
{
	[SerializeField] private float distanceFromSpawningPoint;
	
	public void Update ()
	{
		CheckDirection();
		Move();
	}

	void Move()
	{
		movementManager.MoveBat(direction);
	}

	void CheckDirection()
	{
		if (startingPosition.x - transform.position.x >= distanceFromSpawningPoint)
			ChangeDirection();
		else if (transform.position.x  - startingPosition.x >= distanceFromSpawningPoint)
			ChangeDirection();
		Debug.Log("s"+ startingPosition.x);
		Debug.Log("t" + transform.position.x);
		Debug.Log("d" + distanceFromSpawningPoint);
	}

	void ChangeDirection()
	{
		direction *= -1;
	}
}


