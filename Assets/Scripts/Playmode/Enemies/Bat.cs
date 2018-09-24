using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Bat : EnemyController
{
	public void Update ()
	{
		CheckDirection();
		Move();
	}

	void Move()
	{
		movementManager.MoveBat(direction, ref actualPosition);
	}

	void CheckDirection()
	{
		if (startingPosition.x - actualPosition.x >= movementManager.DistanceFromSpawningPoint)
			ChangeDirection();
		else if (actualPosition.x - startingPosition.x >= movementManager.DistanceFromSpawningPoint)
			ChangeDirection();
	}

	void ChangeDirection()
	{
		direction *= -1;
	}
}


