using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripWireScript : MonoBehaviour
{

	[SerializeField] private GameObject[] triggerables;
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
				triggerable.GetComponent<ITriggerable>()?.Close();
			}
		}
		isTripped = true;
	}


}
