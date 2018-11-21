using System.Collections;
using System.Collections.Generic;
using Game.Puzzle.Light;
using UnityEngine;

public class PerformanceSaver : MonoBehaviour {

	// Use this for initialization
	[SerializeField] private List<MeshLight> FirstHalfLights;
	[SerializeField] private List<MeshLight> SecondHalfLights;
	[SerializeField] private bool PlayerInFirstHalf;
	private BoxCollider2D box;

	private void Awake()
	{
		box = GetComponent<BoxCollider2D>();
	}

	void Start () {

		DisableLights();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		EnableLights();
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		DisableLights();
	}


	private void DisableLights()
	{
		if (PlayerInFirstHalf)
		{
			foreach (var light in SecondHalfLights)
			{
				if (light.UpdateEveryFrame)
				{
					light.transform.root.gameObject.active = false;
				}
			}
		}
		else
		{
			foreach (var light in FirstHalfLights)
			{
				if (light.UpdateEveryFrame)
				{
					light.transform.root.gameObject.active = false;
				}
			}
		}
	}

	private void EnableLights()
	{
		if (PlayerInFirstHalf)
		{
			foreach (var light in SecondHalfLights)
			{
				if (light.UpdateEveryFrame)
				{
					light.transform.root.gameObject.active = true;
				}
			}

			PlayerInFirstHalf = false;
		}
		else
		{
			foreach (var light in FirstHalfLights)
			{
				if (light.UpdateEveryFrame)
				{
					light.transform.root.gameObject.active = true;
				}
			}
			PlayerInFirstHalf = true;
		}
	}
}
