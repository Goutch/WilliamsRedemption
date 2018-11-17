using System.Collections;
using System.Collections.Generic;
using Game.Puzzle;
using Game.Puzzle.Light;
using UnityEngine;
using UnityEngine.Analytics;

public class LightMaster : MonoBehaviour
{

	[Tooltip("Array of lights. Add the lights in the order they should light up.")]
	[SerializeField] private GameObject [] Lights;
	[Tooltip("Amount of time in seconds where each light stays open.")]
	[SerializeField] private float timePerLight;
	[Tooltip("Checking this box will separate the lights in two groups. Lights in a group all have the same state at the same time.")]
	[SerializeField] private bool SeparateLightsInGroups;

	private ITriggerable currentlight;
	private float timeAtStart;
	private int currentLightIndex;

	private void Awake()
	{
		timeAtStart = 0;
		currentLightIndex = 0;
		currentlight = Lights[currentLightIndex].GetComponent<ITriggerable>();
	}

	private void Start()
	{
		if (!SeparateLightsInGroups)
		{
			currentlight.Open();
		}
		else
		{
			bool open = true;
			foreach (var l in Lights)
			{
				currentlight = l.GetComponent<ITriggerable>();
				if (open)
				{
					currentlight.Open();
					open = false;
				}
				else
				{
					currentlight.Close();
					open = true;
				}
			}
		}	
		timeAtStart = Time.time;
	}
	
	private void Update () 
	{
		if (!SeparateLightsInGroups)
		{
			Cycle();
		}
		else
		{
			Alternate();
		}
		
	}

	private void Cycle()
	{
		if(TimeSinceLit() >= timePerLight)
		{
			currentlight.Close();
			currentLightIndex++;
			if (currentLightIndex >= Lights.Length)
			{
				currentLightIndex = 0;
				currentlight = Lights[currentLightIndex].GetComponent<ITriggerable>();
				currentlight.Open();
				timeAtStart = Time.time;
			}
			else
			{
				currentlight = Lights[currentLightIndex].GetComponent<ITriggerable>();
				currentlight.Open();
				timeAtStart = Time.time;
			}
		}
	}

	private void Alternate()
	{
		if (TimeSinceLit() >= timePerLight)
		{
			foreach (var l in Lights)
			{
				currentlight = l.GetComponent<ITriggerable>();

				if (currentlight.IsOpened())
				{
					currentlight.Close();
				}
				else
				{
					currentlight.Open();
				}
			}

			timeAtStart = Time.time;
		}
	}

	private float TimeSinceLit()
	{
		float timeSinceStart = Time.time - timeAtStart;
		return timeSinceStart;
	}
}
