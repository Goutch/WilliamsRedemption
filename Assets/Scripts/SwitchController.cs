using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
//fix le isLocked
	[SerializeField] private GameObject[] triggerables;
	[SerializeField] private float shutDownTime;
	[SerializeField] private bool hasTimer;

	private float timerStartTime;
	private bool isTriggered;
	

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && isTriggered == false)
		{
			
			foreach (var triggerable in triggerables)
			{
				
				if (triggerable.GetComponent<ITriggerable>().CanBeOpened())
				{
					triggerable.GetComponent<ITriggerable>()?.Open();
					if (hasTimer)
					{
						timerStartTime = Time.time;
						isTriggered = true;
					}
				}
				else
				{
					triggerable.GetComponent<ITriggerable>()?.Close();
					if (hasTimer)
					{
						timerStartTime = Time.time;
						isTriggered = true;
					}
				}
			}
		}
	}

	void Start()
	{
		timerStartTime = 0.0f;
		foreach (var triggerable in triggerables)
		{	
			if (triggerable.GetComponent<ITriggerable>().IsOpened())
			{
				triggerable.GetComponent<ITriggerable>()?.Open();
			}
			else
			{
				triggerable.GetComponent<ITriggerable>()?.Close();
			}
		}
		isTriggered = false;
	}
	
	void Update()
	{
		if (hasTimer && isTriggered)
		{
			if (TimeIsUp())
			{
				ChangeSate();
			}
		}
	}
	
	private bool TimeIsUp()
	{
		if (Time.time - timerStartTime >= shutDownTime)
		{
			return true;
		}
		return false;
	}

	private void ChangeSate()
	{
		foreach (var triggerable in triggerables)
		{
			if (triggerable.GetComponent<ITriggerable>().IsOpened())
			{
				triggerable.GetComponent<ITriggerable>()?.Close();
				isTriggered = false;
			}
			else
			{
				triggerable.GetComponent<ITriggerable>()?.Open();
				isTriggered = false;
			}
		}
	}

}
