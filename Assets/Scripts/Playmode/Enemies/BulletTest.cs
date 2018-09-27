using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : EnemyController {

	public void Update ()
	{
		movementManager.MoveBat(direction);
	}
}