using System.Collections;
using System.Collections.Generic;
using Game.Puzzle;
using Game.Puzzle.Light;
using UnityEngine;
using UnityEngine.Analytics;

public class LightMaster : MonoBehaviour
{

	[Tooltip("Array of lights. Add the lights in the order they should light up.")]
	[SerializeField] private GameObject [] lights;
	[Tooltip("Amount of time in seconds where each light stays open.")]
	[SerializeField] private float timePerLight;

	private ITriggerable currentlight;
	private float timeAtStart;
	private int currentLightIndex;

	private void Awake()
	{
		timeAtStart = 0;
		currentLightIndex = 0;
		currentlight = lights[currentLightIndex].GetComponent<ITriggerable>();
	}

	private void Start()
	{
		currentlight.Open();
		timeAtStart = Time.time;
	}
	
	private void Update () 
	{
		Cycle();
	}

	private void Cycle()
	{
		if(TimeSinceLit() >= timePerLight)
		{
			currentlight.Close();
			currentLightIndex++;
			if (currentLightIndex >= lights.Length)
			{
				currentLightIndex = 0;
				currentlight = lights[currentLightIndex].GetComponent<ITriggerable>();
				currentlight.Open();
				timeAtStart = Time.time;
			}
			else
			{
				currentlight = lights[currentLightIndex].GetComponent<ITriggerable>();
				currentlight.Open();
				timeAtStart = Time.time;
			}
		}
	}

	private float TimeSinceLit()
	{
		float timeSinceStart = Time.time - timeAtStart;
		return timeSinceStart;
	}
}
