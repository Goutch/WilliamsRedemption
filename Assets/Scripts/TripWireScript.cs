using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripWireScript : MonoBehaviour
{

	[SerializeField] private GameObject door;

	private bool isTripped;

	private DoorScript _doorScript;

	void Awake()
	{
		_doorScript = door.GetComponent<DoorScript>();
		isTripped = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && !isTripped)
		{
			_doorScript.Close();
			_doorScript.LockDoor();
		}
		isTripped = true;
	}


	// Update is called once per frame
	void Update () {
		
	}
}
