using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

	private bool isOpened;

	private bool canBeOpened;
	// Use this for initialization
	void Start ()
	{
		isOpened = false;
		canBeOpened = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Open()
	{
		if (canBeOpened)
		{
			isOpened = true;
			this.gameObject.SetActive(false);
		}	
	}

	public void Close()
	{
		isOpened = false;
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
