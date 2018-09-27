using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
//fix le isLocked
	[SerializeField] private GameObject[] triggerables;
	
	[SerializeField] private float shutDownTime;
	[SerializeField] private bool hasTimer;

	private bool isOpened;
	private float timerStartTime;
	

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			foreach (var triggerable in triggerables)
			{
				
				if (triggerable.GetComponent<ITriggerable>().CanBeOpened())
				{
					triggerable.GetComponent<ITriggerable>()?.Open();
					isOpened = true;
					if (hasTimer)
					{
						timerStartTime = Time.time;						
					}
				}
			}
		}
	}

	void Awake()
	{
		isOpened = false;
		timerStartTime = 0.0f;
	}
	
	void Update()
	{
		if (hasTimer && isOpened)
		{
			if (timeIsUp())
			{
				foreach (var triggerable in triggerables)
				{
					triggerable.GetComponent<ITriggerable>()?.Close();
				}
			}
		}
	}
	
	private bool timeIsUp()
	{
		if (Time.time - timerStartTime >= shutDownTime)
		{
			return true;
		}
		return false;
	}

}
