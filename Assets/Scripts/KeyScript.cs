using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{

	[SerializeField] private GameObject door;
	private DoorScript doorScript;


	void Awake()
	{
		doorScript = door.GetComponent<DoorScript>();
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			doorScript.UnlockDoor();
		}
		
		gameObject.SetActive(false);
	}
}
