using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

	private bool canBeOpened;
	// Use this for initialization
	void Start ()
	{
		canBeOpened = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Open()
	{
		if (canBeOpened && this.gameObject.activeInHierarchy)
		{
			this.gameObject.SetActive(false);
		}	
	}

	public void Close()
	{
		this.gameObject.SetActive(true);
	}

	public void LockDoor()
	{
		canBeOpened = false;
	}

	public void UnlockDoor()
	{
		canBeOpened = true;
	}
	
}
