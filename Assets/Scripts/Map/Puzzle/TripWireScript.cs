using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripWireScript : MonoBehaviour
{

	[Tooltip("Objects triggered by this trigger.")]
	[SerializeField] private GameObject[] triggerables;
	[Tooltip("Check this box if objects tied to this trigger need to be opened on start")]
	[SerializeField] private bool IsOpened;

	private bool isTripped;

	void Awake()
	{
		isTripped = false;

		if (IsOpened)
		{
			foreach (var triggerable in triggerables)
			{
				triggerable.GetComponent<ITriggerable>()?.Open();	
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && !isTripped)
		{
			foreach (var triggerable in triggerables)
			{
				if (!triggerable.GetComponent<ITriggerable>().IsLocked()&& triggerable.GetComponent<ITriggerable>().IsOpened())
				{
					triggerable.GetComponent<ITriggerable>()?.Close();
					isTripped = true;
					triggerable.GetComponent<ITriggerable>()?.Lock();
				}
				else if(!triggerable.GetComponent<ITriggerable>().IsLocked())
				{
					triggerable.GetComponent<ITriggerable>()?.Open();
					isTripped = true;
					triggerable.GetComponent<ITriggerable>()?.Lock();
				}
				
			}
		}
		
	}


}
