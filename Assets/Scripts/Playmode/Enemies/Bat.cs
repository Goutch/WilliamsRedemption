using System;
using System.Collections;
using System.Collections.Generic;
using Playmode.EnnemyRework;
using UnityEngine;
[CreateAssetMenu(fileName = "BatStrategy", menuName = "EnnemyStrategy/BatStrategy", order = 1)]

public class Bat : Enemy
{
	[SerializeField] private float distanceFromSpawningPoint;
	private RootMover rootMover;
	private int direction = 1;
	private Vector2 startingPosition;

	void Move()
	{
		rootMover.FlyToward(new Vector2(rootMover.transform.position.x+direction*distanceFromSpawningPoint
			,rootMover.transform.position.y),Speed);
	}

	void CheckDirection()
	{
		if(Math.Abs(rootMover.transform.position.x-startingPosition.x)>=distanceFromSpawningPoint)
		{
			ChangeDirection();
		}
	}

	void ChangeDirection()
	{
		direction *= -1;
	}

	public override void Init(GameObject enemyControllerObject)
	{
		rootMover = enemyControllerObject.GetComponent<RootMover>();
		startingPosition = rootMover.transform.position;
		enemyControllerObject.GetComponent<Rigidbody2D>().isKinematic = true;
	}

	public override void Act()
	{
		CheckDirection();
		Move();
	}
}


