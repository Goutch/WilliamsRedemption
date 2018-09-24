using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitchController : MonoBehaviour
{

	[SerializeField] private GameObject door;
	private DoorScript _doorScript;


	void Awake()
	{
		_doorScript = door.GetComponent<DoorScript>();
	}
	

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			_doorScript.Open();
		}
	}

}
